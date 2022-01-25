using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class TaskGameHandler : MonoBehaviour
{
    private static TaskGameHandler gameHandlerInstance;
    public static TaskGameHandler GetInstance()
    {
        return gameHandlerInstance;
    }
    
    private TaskSystem taskSystem;
    public TaskSystem GetTaskSystem()
    {
        return taskSystem;
    }
    [SerializeField] private Sprite PFCherrySprite;
    [SerializeField] private Sprite appleSprite;
    [SerializeField] private Sprite dewSprite;


    private StartEmptyVillagerPool pool;
    [SerializeField]
    private GameObject worker;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(typeof(string).Assembly.ImageRuntimeVersion);
        taskSystem = new TaskSystem();
        pool = FindObjectOfType<StartEmptyVillagerPool>();
        GameObject spawnedWorker = Instantiate(worker);
        spawnedWorker.transform.position = new Vector3(-3, -3f);
        spawnedWorker.GetComponent<TaskWorkerAI>().SetUp(spawnedWorker.GetComponent<Worker>(), taskSystem);
       
        /*
        GameObject spawnedWorker2 = Instantiate(worker);
        spawnedWorker2.transform.position = new Vector3(3, -3f);
        spawnedWorker2.GetComponent<TaskWorkerAI>().SetUp(spawnedWorker2.GetComponent<Worker>(), taskSystem);
        */

        GameObject dewGameObject = SpawnResourceDew(new Vector3(-7, -3.5f));
        GameObject appleGameObject = SpawnResourceApple(new Vector3(7, -3.5f));
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TaskSystem.Task task = new TaskSystem.Task.MoveToPosition { targetPosition = new Vector3(UtilsClass.GetMouseWorldPosition().x, -3f) };
            taskSystem.AddTask(task);
            Debug.Log("tried to add task go to: " + new Vector3(UtilsClass.GetMouseWorldPosition().x, -3f));
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            //GameObject spawnedWorker = Instantiate(worker);
            //spawnedWorker.transform.position = new Vector3(UtilsClass.GetMouseWorldPosition().x, -3f);
            //spawnedWorker.GetComponent<TaskWorkerAI>().SetUp(spawnedWorker.GetComponent<Worker>(), taskSystem);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //TaskSystem.Task task = new TaskSystem.Task.CleanUp { targetPosition = new Vector3(4, -3), cleanUpAction =  ()=> { Debug.Log("Cleaned Up Stuff"); } };
            //taskSystem.AddTask(task);
            //taskSystem.EnqueueTaskHelper()
            SpawnPFCherryCleanUp(new Vector3(UtilsClass.GetMouseWorldPosition().x, -3f));

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TaskSystem.Task task = new TaskSystem.Task.Victory { };
            taskSystem.AddTask(task);
            Debug.Log("tried to do victory");
        }

        
    }

    public TaskSystem GetTasksystem()
    {
        return taskSystem;
    }


    private void SpawnPFCherryCleanUp(Vector3 position)
    {
        GameObject PFCherryObj = SpawnResourcePFCherry(position);

        float cleanupTime = Time.time + 5f;
        taskSystem.EnqueueTaskHelper(() => 
        { 
            if (Time.time > cleanupTime) 
            {
                TaskSystem.Task task = new TaskSystem.Task.CleanUp { targetPosition = position, cleanUpAction = () => { PFCherryObj.SetActive(false); Debug.Log("Cleaned Up Cherry"); } };
                return task;
            }
            else
            {
                return null;
            }        
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
}
