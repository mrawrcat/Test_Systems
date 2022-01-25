using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class GameHandler : MonoBehaviour
{

   
    [SerializeField]
    private Transform QueueStartTransform; 
    [SerializeField]
    private Transform ToiletBuildingEndTransform;
    [SerializeField]
    float positionSize = 1.5f;
    private WaitingQueue waitingQueue;
    private Vector3 firstPos;
    private Vector3 toiletExit;
    private List<Vector3> waitingQueuePosList = new List<Vector3>(); //for some reason creates a list of vector3 and then waiting queue uses this list
    // Start is called before the first frame update
    void Start()
    {
        firstPos = QueueStartTransform.position;
        toiletExit = ToiletBuildingEndTransform.position;
        
        for (int i = 0; i < 5; i++)
        {
            waitingQueuePosList.Add(firstPos - new Vector3(1f, 0) * positionSize * i);
        }
        
        waitingQueue = new WaitingQueue(waitingQueuePosList);

        //waitingQueue.AddPosition(waitingQueuePosList[waitingQueuePosList.Count - 1] + new Vector3(0, -positionSize));

        CMDebug.ButtonUI(new Vector2(-50, 200), "", waitingQueue.AddPositionLeft );
        
        waitingQueue.OnVillagerArrivedAtFrontofQueue += WaitingQueue_OnVillagerArrivedAtFrontOfQueue;
        waitingQueue.OnVillagerAdded += WaitingQueue_OnVillagerAdded;

        List<Vector3> buildingPosList = new List<Vector3>() { new Vector3(20,-3.5f), new Vector3(25, -3.5f) };//toilet positions
        //TestBuilding testBuilding = new TestBuilding(waitingQueue, buildingPosList, toiletExit);
       
        
    }

    private void Update()
    {
    }

    public void AddPosLeft()
    {
        waitingQueue.AddPosition(waitingQueuePosList[waitingQueuePosList.Count - 1] + new Vector3(-positionSize, 0));
    }

    public void DoAddGuest(Villager villager)
    {
        if (waitingQueue.canAddVillager())
        {
            waitingQueue.AddVillager(villager);
        }
    }

    public void SendGuest()
    {
        Villager villager = waitingQueue.GetFirstInQueue();
        if(villager != null)
        {
            villager.MoveTo(new Vector3(-5, villager.transform.position.y));
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
