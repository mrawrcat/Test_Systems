using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class TaskGameHandler : MonoBehaviour
{
    private static TaskGameHandler gameHandlerInstance;
    public static TaskGameHandler GetInstance()
    {
        return gameHandlerInstance;
    }
    
    private TaskSystem<Task> taskSystem;
    public TaskSystem<TransporterTask> transporterTaskSystem;
    public TaskSystem<TestTask> testTaskSystem;
    //public TaskSystem<Task_IEnemy_Unit> enemyUnitTaskSystem;

    [SerializeField] private Sprite PFCherrySprite;
    [SerializeField] private Sprite appleSprite;
    [SerializeField] private Sprite dewSprite;


    private DepositSlot depositSlot;
    [SerializeField]
    private GameObject worker;
    private GameObject spawnedWorkerSave;

    [SerializeField] private Transform Archer_Station;
    [SerializeField] private List<TaskTestNewWorkerAI> TaskTestWorkerAIList;

    // Start is called before the first frame update
    private void Awake()
    {
        TaskTestWorkerAIList = new List<TaskTestNewWorkerAI>();
        taskSystem = new TaskSystem<Task>();
        transporterTaskSystem = new TaskSystem<TransporterTask>();
        testTaskSystem = new TaskSystem<TestTask>();
        //enemyUnitTaskSystem = new TaskSystem<Task_IEnemy_Unit>();
        //GameObject spawnedWorker = Instantiate(worker);
        //spawnedWorker.transform.position = new Vector3(0, -3f);
        //spawnedWorker.GetComponent<TaskWorkerAI>().SetUp(spawnedWorker.GetComponent<Worker>(), taskSystem);
       
        /*
        spawnedWorkerSave = spawnedWorker.gameObject;
        GameObject spawnedWorker2 = Instantiate(worker);
        spawnedWorker2.transform.position = new Vector3(3, -3f);
        spawnedWorker2.GetComponent<TaskWorkerAI>().SetUp(spawnedWorker2.GetComponent<Worker>(), taskSystem);
        GameObject dewGameObject = SpawnResourceDew(new Vector3(-7, -3.5f));
        TaskSystem.Task task = new TaskSystem.Task.TakeResourceToPosition
        {
            resourcePosition = dewGameObject.transform.position,
            resourceDepositPosition = appleGameObject.transform.position,
            takeResource = (TaskWorkerAI dewtaskWorkerAI) => { dewGameObject.transform.SetParent(dewtaskWorkerAI.transform); },
            dropResource = () => { dewGameObject.transform.SetParent(null); },
        };
        taskSystem.AddTask(task);
        task = new TaskSystem.Task.MoveToPosition { targetPosition = new Vector3(-8, -3f) };
        taskSystem.AddTask(task);
        */

        GameObject appleGameObject = SpawnResourceApple(new Vector3(7, -3.5f));
        depositSlot = new DepositSlot(appleGameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TestTask task = new TestTask.MoveToPosition { targetPosition = new Vector3(UtilsClass.GetMouseWorldPosition().x, -4f) };
            testTaskSystem.AddTask(task);
            //Task task = new Task.MoveToPosition { targetPosition = new Vector3(UtilsClass.GetMouseWorldPosition().x, -3f) };
            //taskSystem.AddTask(task);
            /*
            Debug.Log("tried to add task go to: " + new Vector3(UtilsClass.GetMouseWorldPosition().x, -3f));
            */
        }

        if (Input.GetMouseButtonDown(1))
        {
            SpawnDewPickUp(new Vector3(UtilsClass.GetMouseWorldPosition().x, -3.5f));
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach(TaskTestNewWorkerAI worker in TaskTestWorkerAIList)
            {
                TestTask task = new TestTask.MoveToPositionThenDie
                {
                    targetPosition = new Vector3(UtilsClass.GetMouseWorldPosition().x, -4f),
                    DieAction = (TaskTestNewWorkerAI) =>
                    {
                        TaskTestWorkerAIList.RemoveAt(TaskTestWorkerAIList.IndexOf(TaskTestNewWorkerAI));
                        TaskTestNewWorkerAI.gameObject.SetActive(false);
                    }
                };
                testTaskSystem.AddTask(task);
            }
            //TaskSystem.Task task = new TaskSystem.Task.CleanUp { targetPosition = new Vector3(4, -3), cleanUpAction =  ()=> { Debug.Log("Cleaned Up Stuff"); } };
            //taskSystem.AddTask(task);
            //taskSystem.EnqueueTaskHelper()
            //SpawnPFCherryCleanUp(new Vector3(UtilsClass.GetMouseWorldPosition().x, -3f));

        }
        if (Input.GetKeyDown(KeyCode.E))
        {

            GameObject spawnedWorker = Instantiate(worker);
            spawnedWorker.transform.position = new Vector3(UtilsClass.GetMouseWorldPosition().x, -4f);
            spawnedWorker.GetComponent<TaskTestNewWorkerAI>().SetUp(spawnedWorker.GetComponent<BaseUnit>(), testTaskSystem);
            TaskTestWorkerAIList.Add(spawnedWorker.GetComponent<TaskTestNewWorkerAI>());
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            Task task = new Task.Victory { };
            taskSystem.AddTask(task);
            Debug.Log("tried to do victory");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ConvertTaskWorkerToTransporter(Archer_Station.position);
            //spawnedWorkerSave.GetComponent<TaskWorkerAI>().enabled = false;
            //spawnedWorkerSave.GetComponent<Transporter_TaskWorkerAI>().enabled = true;
            //spawnedWorkerSave.GetComponent<Transporter_TaskWorkerAI>().SetUp(spawnedWorkerSave.GetComponent<Worker>(), transporterTaskSystem);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            //spawnedWorkerSave.GetComponent<Transporter_TaskWorkerAI>().enabled = false;
            //spawnedWorkerSave.GetComponent<TaskWorkerAI>().enabled = true;
            //spawnedWorkerSave.GetComponent<TaskWorkerAI>().SetUp(spawnedWorkerSave.GetComponent<Worker>(), taskSystem);
        }

        
    }



    private void SpawnPFCherryCleanUp(Vector3 position)
    {
        GameObject PFCherryObj = SpawnResourcePFCherry(position);

        float cleanupTime = Time.time + 5f;
        taskSystem.EnqueueTaskHelper(() => 
        { 
            if (Time.time > cleanupTime) 
            {
                Task task = new Task.CleanUp { targetPosition = position, cleanUpAction = () => { PFCherryObj.SetActive(false); Debug.Log("Cleaned Up Cherry"); } };
                return task;
            }
            else
            {
                return null;
            }        
        });
    }

    private void SpawnDewPickUp(Vector3 position)
    {
        GameObject dewGameObject = SpawnResourceDew(position);
        
        taskSystem.EnqueueTaskHelper(()=>
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
    private void ConvertTaskWorkerToTransporter(Vector3 position)
    {
        taskSystem.EnqueueTaskHelper(()=>
        {
            Task task = new Task.ConvertToTransporterTask
            {
                buildingPosition = position,
                convertAction = (TaskWorkerAI convertTaskWorkerAI) => 
                { 
                    convertTaskWorkerAI.GetComponent<TaskWorkerAI>().enabled = false; 
                    convertTaskWorkerAI.GetComponent<Transporter_TaskWorkerAI>().enabled = true;
                    convertTaskWorkerAI.GetComponent<Transporter_TaskWorkerAI>().SetUp(convertTaskWorkerAI.GetComponent<Worker>(), transporterTaskSystem);
                },
            };
            return task;
        });
        
        
    }

    private GameObject SpawnResourcePFCherry(Vector3 position)
    {
        GameObject gameObject = new GameObject("PFCherry", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = PFCherrySprite;
        gameObject.transform.position = position;
        return gameObject;
    }
    
    private GameObject SpawnResourceApple(Vector3 position)
    {
        GameObject gameObject = new GameObject("Apple", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = appleSprite;
        gameObject.transform.position = position;
        return gameObject;
    }
    private GameObject SpawnResourceDew(Vector3 position)
    {
        GameObject gameObject = new GameObject("Dew", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = dewSprite;
        gameObject.transform.position = position;
        return gameObject;
    }

    public class DepositSlot
    {
        private Transform depositObjTransform;
        private Transform depositSlotTransform;
        private bool hasDepositIncoming;

        public DepositSlot(Transform depositSlotTransform)
        {
            this.depositSlotTransform = depositSlotTransform;
            SetDepositTransform(null);
        }

        public bool isEmpty()
        {
            return depositObjTransform == null && !hasDepositIncoming;
        }

        public void SetDepositIncoming(bool hasDepositIncoming)
        {
            this.hasDepositIncoming = hasDepositIncoming;
        }

        public void SetDepositTransform(Transform depositObjTransform)
        {
            this.depositObjTransform = depositObjTransform;
            hasDepositIncoming = false;
            UpdateSprite();
            FunctionTimer.Create(()=> 
            {
                if(depositObjTransform != null)
                {
                    Destroy(depositObjTransform.gameObject);
                    SetDepositTransform(null);
                }
            }, 1f);
        }

        public Vector3 GetPosition()
        {
            return depositSlotTransform.position;
        }

        public void UpdateSprite()
        {
            depositSlotTransform.GetComponent<SpriteRenderer>().color = isEmpty() ? Color.grey : Color.red;
        }
    }


    public class Task : TaskBase
    {
        public class MoveToPosition : Task
        {
            public Vector3 targetPosition;

        }

        public class Victory : Task
        {

        }

        public class CleanUp : Task
        {
            public Vector3 targetPosition;
            public Action cleanUpAction;
        }

        public class TakeResourceToPosition : Task //grabs a resource and takes it to building? position
        {
            public Vector3 resourcePosition;
            public Action<TaskWorkerAI> takeResource;
            public Vector3 resourceDepositPosition; //position where worker deposits resource
            public Action dropResource;
        }

        public class ConvertToTransporterTask : Task
        {
            public Vector3 buildingPosition;
            public Action<TaskWorkerAI> convertAction;
        }
    }

    public class TransporterTask : TaskBase
    {
        public class MoveToPosition : TransporterTask
        {
            public Vector3 targetPosition;

        }

        public class TakeWeaponFromSlotToPosition : TransporterTask
        {
            public Vector3 resourcePosition;
            public Action<Transporter_TaskWorkerAI> takeResource;
            public Vector3 resourceDepositPosition; //position where worker deposits resource
            public Action dropResource;
        }
    }

    public class GhostTask : TaskBase
    {
        public class MoveToPosition : GhostTask
        {
            public Vector3 targetPosition;

        }

        public class FinishTutorial : GhostTask
        {
            public Vector3 targetPosition;
            public Action finishAction;
        }
    }

    public class Task_IEnemy_Unit : TaskBase
    {
        public class MoveToPosition : Task_IEnemy_Unit
        {
            public Vector3 targetPosition;

        }

        public class Victory : Task_IEnemy_Unit
        {

        }

        public class CleanUp : Task_IEnemy_Unit
        {
            public Vector3 targetPosition;
            public Action cleanUpAction;
        }

        public class TakeResourceToPosition : Task_IEnemy_Unit //grabs a resource and takes it to building? position
        {
            public Vector3 resourcePosition;
            public Action<Enemy_Spearman_AI> takeResource;
            public Vector3 resourceDepositPosition; //position where worker deposits resource
            public Action dropResource;
        }

        public class ConvertToTransporterTask : Task_IEnemy_Unit
        {
            public Vector3 buildingPosition;
            public Action<Enemy_Spearman_AI> convertAction;
        }

        public class GetHitThenContinueToTarget : Task_IEnemy_Unit
        {
            public Action<Enemy_Spearman_AI> stopAction;
        }
    }

    public class TestTask : TaskBase
    {
        public class MoveToPosition : TestTask
        {
            public Vector3 targetPosition;

        }
        public class MoveToPositionThenDie : TestTask
        {
            public Vector3 targetPosition;
            public Action<TaskTestNewWorkerAI> DieAction;

        } 
        public class StopAndAttack : TestTask
        {
            public Action<TaskTestNewWorkerAI> AttackAction;
        }
    }
}
 

