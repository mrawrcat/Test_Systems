using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
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
    private List<GameObject> resourceObjList; //this probably cant be used unless i find a way to remove when object is destroyed
    
    // Start is called before the first frame update
    void Start()
    {
        taskGameHandler = FindObjectOfType<TaskGameHandler>();
        GameObject depositSlotObj = SpawnDepositSlot(transform.position + new Vector3(10,0));
        resourceDepositSlot = new TaskGameHandler.DepositSlot(depositSlotObj.transform);
        resourceObjList = new List<GameObject>();
        nextSpawnTime = spawnRate;
        finalRandomPos = transform.position + new Vector3(Random.Range(-3, 3), 0);
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
        //resourceObjList.Add(resourceGameObject);
        taskGameHandler.villagerTaskSystem.EnqueueTaskHelper(() => 
        {
            if (resourceDepositSlot.isEmpty())
            {
                resourceDepositSlot.SetDepositIncoming(true);
                TaskGameHandler.TestTaskVillager gatherTask = new TaskGameHandler.TestTaskVillager.TakeResourceFromSlotToPosition
                {
                    resourcePosition = resourceGameObject.transform.position,
                    resourceDepositPosition = resourceDepositSlot.GetPosition(),
                    takeResource = (TaskTestVillagerAI villagerAI) => { resourceGameObject.transform.SetParent(villagerAI.transform); resourceGameObject.transform.position = villagerAI.transform.position + new Vector3(0, 2); },
                    dropResource = () =>
                    {
                        resourceGameObject.transform.SetParent(null);
                        //resourceGameObject.transform.position = resourceDepositSlot.GetPosition();
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
    /*
    private void SpawnDewPickUp(Vector3 position)
    {
        GameObject dewGameObject = SpawnResourceDew(position);

        taskSystem.EnqueueTaskHelper(() =>
        {
            if (depositSlot.isEmpty())
            {
                depositSlot.SetDepositIncoming(true);
                Task task = new Task.TakeResourceToPosition
                {
                    resourcePosition = dewGameObject.transform.position,
                    resourceDepositPosition = depositSlot.GetPosition(),
                    takeResource = (TaskWorkerAI dewtaskWorkerAI) => { dewGameObject.transform.SetParent(dewtaskWorkerAI.transform); },
                    dropResource = () =>
                    {
                        dewGameObject.transform.SetParent(null);
                        depositSlot.SetDepositTransform(dewGameObject.transform);
                    },
                };
                return task;
            }
            else
            {
                return null;
            }
        });
    }
    */

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, detectSize);
    }


}
