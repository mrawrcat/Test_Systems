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
    private float nextSpawnTime;
    private Vector3 finalRandomPos;
    private TaskGameHandler taskGameHandler;
    private TaskGameHandler.DepositSlot resourceDepositSlot;

    [Header("Queue Stuff")]
    //[SerializeField] private Transform QueueStartTransform;
    [SerializeField] private float positionSize = 1.5f;
    [SerializeField] private int maxSpots;
    private GatherWaitingQueue gatherWaitingQueue;
    public GatherWaitingQueue GetGatherWaitingQueue()
    {
        return gatherWaitingQueue;
    }
    private Vector3 firstPos;
    private List<Vector3> gatherWaitingQueuePosList = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        taskGameHandler = FindObjectOfType<TaskGameHandler>();
        GameObject depositSlotObj = SpawnDepositSlot(transform.position + new Vector3(10,0));
        resourceDepositSlot = new TaskGameHandler.DepositSlot(depositSlotObj.transform);
        nextSpawnTime = spawnRate;
        finalRandomPos = transform.position + new Vector3(Random.Range(-3, 3), 0);
        firstPos = resourceDepositSlot.GetPosition() + new Vector3(-1,0);

        for(int i = 0; i < 2; i++)
        {
            BaseUnit.Create_BaseUnit(transform.position + new Vector3(-20 - i,0), transform.position, BaseUnit.UnitType.Villager);
        }

        for (int i = 0; i < maxSpots; i++)
        {
            gatherWaitingQueuePosList.Add(firstPos - new Vector3(1f, 0) * positionSize * i);
        }
        gatherWaitingQueue = new GatherWaitingQueue(gatherWaitingQueuePosList);
        gatherWaitingQueue.OnUnitAdded += GatherWaitingQueue_OnVillagerAdded;
        gatherWaitingQueue.OnUnitArrivedAtFrontofQueue += GatherWaitingQueue_OnUnitArrivedAtFrontOfQueue;

        List<Vector3> DepositSlotPosList = new List<Vector3>() { resourceDepositSlot.GetPosition() };//toilet positions
        //TestBuilding testBuilding = new TestBuilding(waitingQueue, buildingPosList, toiletExit);
        /*
        */
    }

    // Update is called once per frame
    void Update()
    {
        SpawnResourceOnTimer(transform.position);
    }
    private GameObject SpawnResourcePFCherry(Vector3 position) //resources need a collider so that resource spawner can detect them
    {
        GameObject gameObject = new GameObject("PFCherry", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = PFCherrySprite;
        gameObject.layer = LayerMask.NameToLayer("CherryResource");
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
            if (resourceDepositSlot.isEmpty())
            {
                resourceDepositSlot.SetDepositIncoming(true);

                TaskGameHandler.TestTaskVillager gatherTask = new TaskGameHandler.TestTaskVillager.TakeResourceFromSlotToPosition
                {
                    resourcePosition = resourceGameObject.transform.position,
                    resourceDepositPosition = resourceDepositSlot.GetPosition(),
                    takeResource = (TaskTestVillagerAI villagerAI) => 
                    { 
                        resourceGameObject.transform.SetParent(villagerAI.transform); 
                        resourceGameObject.transform.position = villagerAI.transform.position + new Vector3(0, 2); 
                    },
                    dropResource = () => //actual action of drop resource
                    {
                        resourceGameObject.transform.SetParent(null);
                        // resourceGameObject.transform.position = resourceDepositSlot.GetPosition();
                        resourceDepositSlot.SetDepositTransform(resourceGameObject.transform);
                    },
                };
                return gatherTask;
            }
            else
            {
                return null;
            }
        });
        /*
        */

        }

    private void SpawnResourceOnTimer(Vector3 resourcePos)
    {
        Collider2D[] resources = Physics2D.OverlapBoxAll(transform.position, detectSize, 0, whatIsResource);//resources dont have collider so it doesnt detect
        Debug.Log("amount of resources in detect: " + resources.Length);
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
                            Debug.Log("current final random Pos" + finalRandomPos);
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
        if (gatherWaitingQueue.CanAddUnit())
        {
            gatherWaitingQueue.AddUnit(baseUnit);
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
