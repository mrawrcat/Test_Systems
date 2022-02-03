using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class OrbitHandler : MonoBehaviour
{
    [SerializeField] private Sprite followerSprite;
    [SerializeField] private RuntimeAnimatorController animatorController;
    [SerializeField] private Transform orbitTransform;
    [SerializeField] private float distanceFromCenter = 1;
    [SerializeField] private int maxFollowers = 7;
    [SerializeField] private Transform[] queuePositions;
    private int currentFollowers;
    private float radius = 1;
    private List<Transform> followerTransformList = new List<Transform>();
    private OrbitTransformQueue orbitQueue;

    // Start is called before the first frame update
    void Start()
    {
        queuePositions = new Transform[maxFollowers];
        for (int i = 0; i < maxFollowers; i++)
        {
            GameObject newPosition = new GameObject();
            newPosition.name = "newPosition " + i.ToString();
            newPosition.transform.SetParent(orbitTransform);
            //find out position here
            newPosition.transform.position = GetPositon(orbitTransform);
            queuePositions[i] = newPosition.transform;
            followerTransformList.Add(queuePositions[i]);
            currentFollowers++;
        }
        
        orbitQueue = new OrbitTransformQueue(followerTransformList);
        orbitQueue.OnFollowerArrivedAtFrontofQueue += WaitingQueue_OnVillagerArrivedAtFrontOfQueue;
        orbitQueue.OnFollowerAdded += WaitingQueue_OnVillagerAdded;
        
    }

    // Update is called once per frame
    void Update()
    {

        

        if (Input.GetMouseButtonDown(1))
        {
            //spawn a follower
            SpawnFollower(UtilsClass.GetMouseWorldPosition());
        }

        /*
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            for(int i = 0; i < followerList.Count; i++)
            {
                if(followerList[i].transform.parent != orbitTransform)
                {
                    if(currentFollowers < maxFollowers)
                    {
                        GameObject setTransform = new GameObject();
                        setTransform.transform.SetParent(orbitTransform);
                        setTransform.transform.position = GetPositon(orbitTransform);
                        followerList[i].GetComponent<Follower>().MoveToTransform(setTransform.transform);
                        followerList[i].gameObject.transform.SetParent(orbitTransform);
                        currentFollowers++;
                        break;
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            for (int i = 0; i < followerList.Count; i++)
            {
                if (followerList[i].transform.parent == orbitTransform)
                {
                    if (currentFollowers > 0)
                    {
                        followerList[i].gameObject.transform.SetParent(transform);
                        followerList[i].GetComponent<Follower>().setCurrentTransformToNull();
                        currentFollowers--;
                        break;
                    }
                }
            }
        }
        */
    }

    public void DoAddGuest(Follower follower)
    {
        if (orbitQueue.canAddFollower())
        {
            orbitQueue.AddFollower(follower);
        }
    }

    public void SendGuest()
    {
        Follower follower = orbitQueue.GetFirstInQueue();
        if (follower != null)
        {
            follower.MoveTo(new Vector3(0, follower.transform.position.y));
        }
        else
        {
            Debug.Log("no villagers in queue");
        }
    }

    public Vector3 GetPositon(Transform objectPos)
    {
        float angle = currentFollowers * Mathf.PI * 2f / maxFollowers;
        return objectPos.position + new Vector3(Mathf.Sin(angle) * radius * distanceFromCenter, Mathf.Cos(angle) * radius * distanceFromCenter, 0);
    }

    private void SpawnFollower(Vector3 position)
    {
        GameObject followerObj = FollowerObj(position);
        //followerTransformList.Add(followerObj.transform);
        //Debug.Log(followerTransformList.Count);
    }


    private GameObject FollowerObj(Vector3 position)
    {
        GameObject gameObject = new GameObject("Follower", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = followerSprite;
        gameObject.layer = 6;
        gameObject.transform.position = position;
        gameObject.AddComponent<CircleCollider2D>();
        gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
        gameObject.transform.SetParent(transform);
        gameObject.AddComponent<Follower>();
        gameObject.AddComponent<Animator>();
        gameObject.GetComponent<Animator>().runtimeAnimatorController = animatorController;
        gameObject.AddComponent<TaskWorkerAI>();
        //gameObject.GetComponent<TaskWorkerAI>().SetUp(gameObject.GetComponent<Follower>(), taskSystem);
        return gameObject;
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
