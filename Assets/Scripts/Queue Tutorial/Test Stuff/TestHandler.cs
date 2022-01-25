using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHandler : MonoBehaviour
{
    [SerializeField]
    private Transform QueueStartTransform;
    [SerializeField]
    float positionSize = 1f;
    private Transform firstPos;
    private TestQueue testQueue;
    private List<Transform> testQueuePosList = new List<Transform>();
    [SerializeField]
    private Transform[] QueuePositions = new Transform[5];
    // Start is called before the first frame update
    void Start()
    {
        firstPos = QueueStartTransform;
        for (int i = 0; i < 5; i++)
        {
            GameObject newPosition = new GameObject();
            newPosition.name = "newPosition " + i.ToString();
            newPosition.transform.SetParent(QueueStartTransform);
            /*
            if(i != 0)
            {
                newPosition.transform.SetParent(GameObject.Find("newposition" + (i-1).ToString()).transform);
            }
            */
            newPosition.transform.position = new Vector3(QueueStartTransform.position.x + positionSize * i, -3);
            QueuePositions[i] = newPosition.transform;
            testQueuePosList.Add(QueuePositions[i]);
        }
        testQueue = new TestQueue(testQueuePosList);
        testQueue.OnVillagerArrivedAtFrontofQueue += WaitingQueue_OnVillagerArrivedAtFrontOfQueue;
        testQueue.OnVillagerAdded += WaitingQueue_OnVillagerAdded;
    }
    private void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            if(QueueStartTransform.localScale.x > 0)
            {
                QueuePositions[i].position = new Vector3(QueueStartTransform.position.x + positionSize * i, -3);
            }
            else if(QueueStartTransform.localScale.x < 0)
            {
                QueuePositions[i].position = new Vector3(QueueStartTransform.position.x - positionSize * i, -3);
            }
            
        }

        
    }

    public void DoAddGuest(Villager villager)
    {
        if (testQueue.canAddVillager())
        {
            testQueue.AddVillager(villager);
        }
    }

    public void SendGuest()
    {
        Villager villager = testQueue.GetFirstInQueue();
        if (villager != null)
        {
            villager.MoveTo(new Vector3(0, villager.transform.position.y));
        }
        else
        {
            Debug.Log("no villagers in queue");
        }
    }
    private void WaitingQueue_OnVillagerArrivedAtFrontOfQueue(object sender, System.EventArgs e)
    {
        Debug.Log("Villager Arrived In Front of Queue");
    }

    private void WaitingQueue_OnVillagerAdded(object sender, System.EventArgs e)
    {
        Debug.Log("Villager Added");
    }

}
