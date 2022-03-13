using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Resource Spawn Stuff")]
    [SerializeField] private Sprite PFCherrySprite;
    [SerializeField] private Sprite DepositSlotSprite;
    [SerializeField] private Vector2 detectSize;
    [SerializeField] private LayerMask whatIsResource;
    [SerializeField] private int maxResourceContain;
    [SerializeField] private float spawnRate;
    [SerializeField] private int Num_Of_Units_To_Create_At_Start;
    private float nextSpawnTime;
    private Vector3 finalRandomPos;

    private float waitingTimer;

    private TaskGameHandler taskGameHandler;
    private TaskGameHandler.DepositSlot resourceDepositSlot;
    public TaskGameHandler.DepositSlot GetResourceDepositSlot()
    {
        return resourceDepositSlot;
    }
    [Header("Queue Stuff")]
    //[SerializeField] private Transform QueueStartTransform;
    [SerializeField] private float positionSize = 1.5f;
    [SerializeField] private int maxSpots = 5; //spots should be more or equal to num of units to create at start to prevent bugs for now
    private GatherWaitingQueue gatherWaitingQueue;
    public GatherWaitingQueue GetGatherWaitingQueue()
    {
        return gatherWaitingQueue;
    }
    private Vector3 firstPos;
    private List<Vector3> gatherWaitingQueuePosList = new List<Vector3>();

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    private static List<ResourceSpawner> activeResourceSpawnerList;
    public static ResourceSpawner GetClosestResourceSpawner(Vector3 position, float maxRange)//maybe dont need the maxRange because want to detect all spawners?
    {
        ResourceSpawner closest = null;
        foreach (ResourceSpawner spawner in activeResourceSpawnerList)
        {
            if (Vector3.Distance(position, spawner.GetPosition()) <= maxRange)
            {
                if (closest == null)
                {
                    closest = spawner;
                }
                else
                {
                    float currentClosest = Vector3.Distance(position, closest.transform.position);
                    float currentChecking = Vector3.Distance(position, spawner.transform.position);
                    if (currentChecking < currentClosest)
                    {
                        closest = spawner;
                    }
                }
            }

        }
        return closest;
    }

    private void Awake()
    {
        if (activeResourceSpawnerList == null)
        {
            activeResourceSpawnerList = new List<ResourceSpawner>();
        }
        activeResourceSpawnerList.Add(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        taskGameHandler = FindObjectOfType<TaskGameHandler>();
        GameObject depositSlotObj = SpawnDepositSlot(transform.position + new Vector3(15, -1));
        resourceDepositSlot = new TaskGameHandler.DepositSlot(depositSlotObj.transform, gatherWaitingQueue);
        nextSpawnTime = spawnRate;
        finalRandomPos = transform.position + new Vector3(Random.Range(-3, 3), 0);
        firstPos = resourceDepositSlot.GetPosition() + new Vector3(-1.5f, 0); //front of line i think, no need to touch y here

        for(int i = 0; i < Num_Of_Units_To_Create_At_Start; i++)
        {
            BaseUnit.Create_BaseUnit(transform.position + new Vector3(-20 - i, 0), transform.position, BaseUnit.UnitType.Villager);
        }

        for (int i = 0; i < maxSpots; i++)
        {
            gatherWaitingQueuePosList.Add(firstPos - new Vector3(1f, 0) * positionSize * i);
        }
        gatherWaitingQueue = new GatherWaitingQueue(gatherWaitingQueuePosList);
        gatherWaitingQueue.OnUnitAdded += GatherWaitingQueue_OnVillagerAdded;
        gatherWaitingQueue.OnUnitArrivedAtFrontofQueue += GatherWaitingQueue_OnUnitArrivedAtFrontOfQueue;

        List<Vector3> DepositSlotPosList = new List<Vector3>() { resourceDepositSlot.GetPosition() };//deposit slot/box positions
        GatherWaitingQueueDeposit depositBuilding = new GatherWaitingQueueDeposit(gatherWaitingQueue, DepositSlotPosList, resourceDepositSlot.GetPosition() + new Vector3(3, 0), this);
        Debug.Log("Deposit Slot List: " + DepositSlotPosList);
        //TestBuilding testBuilding = new TestBuilding(waitingQueue, buildingPosList, toiletExit);
        /*
        */
    }

    // Update is called once per frame
    void Update()
    {
        SpawnResourceOnTimer(transform.position);

        waitingTimer -= Time.deltaTime;
        if (waitingTimer <= 0)
        {
            float waitingTimerMax = .2f;
            //resourceDepositSlot.CallNextFirstInLine();
            
            Debug.Log("someone waiting in front of line");
            
            waitingTimer = waitingTimerMax;
        }
    }
    private GameObject SpawnResourcePFCherry(Vector3 position) //resources need a collider so that resource spawner can detect them
    {
        GameObject gameObject = new GameObject("PFCherry", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = PFCherrySprite;
        gameObject.layer = LayerMask.NameToLayer("Resource");
        gameObject.AddComponent<ResourceObject>();
        gameObject.AddComponent<CircleCollider2D>();
        gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
        gameObject.transform.position = position;
        return gameObject;
    }
    private GameObject SpawnDepositSlot(Vector3 position)
    {
        GameObject gameObject = new GameObject("DepositSlot", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = DepositSlotSprite;
        gameObject.transform.position = position;
        return gameObject;
    }


    private void SpawnResourceSendTask(Vector3 resourcePos)
    {
        GameObject resourceGameObject = SpawnResourcePFCherry(resourcePos);

        taskGameHandler.villagerTaskSystem.EnqueueTaskHelper(() => 
        {
            //resourceDepositSlot.SetDepositIncoming(true);
            TaskClasses.TestTaskVillager gatherTask = new TaskClasses.TestTaskVillager.TakeResourceFromSlotToPosition
            {
                resourcePosition = resourceGameObject.transform.position,
                //resourceDepositPosition = resourceDepositSlot.GetPosition(),
                takeResource = (TaskTestVillagerAI villagerAI) => 
                { 
                    resourceGameObject.transform.SetParent(villagerAI.transform); 
                    resourceGameObject.transform.position = villagerAI.transform.position + new Vector3(0, 1); 
                },
            };
            return gatherTask;
        });
        /*
        */

        }

    private void SpawnResourceOnTimer(Vector3 resourcePos)
    {
        Collider2D[] resources = Physics2D.OverlapBoxAll(transform.position, detectSize, 0, whatIsResource);
        //Debug.Log("amount of resources in detect: " + resources.Length);
        if(resources.Length < maxResourceContain)
        {
            nextSpawnTime -= Time.deltaTime;
            if(nextSpawnTime <= 0)
            {
                if(resources.Length > 0)
                {
                    foreach(Collider2D resource in resources)
                    {
                        if(resource.transform.position == finalRandomPos)
                        {
                            finalRandomPos = resourcePos + new Vector3(Random.Range(-3, 3), 0);
                            //Debug.Log("current final random Pos" + finalRandomPos);
                        }
                        else
                        {
                            SpawnResourceSendTask(finalRandomPos);
                            nextSpawnTime = spawnRate;
                        }
                    }
                }
                else
                {
                    SpawnResourceSendTask(finalRandomPos);
                    nextSpawnTime = spawnRate;
                }
                
            }
        }
    }
    public void AddPosLeft()
    {
        gatherWaitingQueue.AddPosition(gatherWaitingQueuePosList[gatherWaitingQueuePosList.Count - 1] + new Vector3(-positionSize, 0));
    }
    public void DoAddGuest(BaseUnit baseUnit)
    {
        if (gatherWaitingQueue.CanAddVillager())
        {
            gatherWaitingQueue.AddVillager(baseUnit);
        }
    }

    public void SendGuest()
    {
        BaseUnit baseUnit = gatherWaitingQueue.GetFirstInQueue();
        if (baseUnit != null)
        {
            baseUnit.MoveTo(new Vector3(-5, baseUnit.transform.position.y));
        }
        else
        {
            Debug.Log("no units in queue");
        }
    }

    private void GatherWaitingQueue_OnUnitArrivedAtFrontOfQueue(object sender, System.EventArgs e)
    {
        Debug.Log("Unit Arrived In Front of Queue");
    }
    private void GatherWaitingQueue_OnVillagerAdded(object sender, System.EventArgs e)
    {
        Debug.Log("Unit Added To Queue");
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, detectSize);
    }


}
