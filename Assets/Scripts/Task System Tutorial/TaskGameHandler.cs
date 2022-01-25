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
       
        GameObject spawnedWorker2 = Instantiate(worker);
        spawnedWorker2.transform.position = new Vector3(3, -3f);
        spawnedWorker2.GetComponent<TaskWorkerAI>().SetUp(spawnedWorker2.GetComponent<Worker>(), taskSystem);
        /*
        */


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
            GameObject spawnedWorker = Instantiate(worker);
            spawnedWorker.transform.position = new Vector3(UtilsClass.GetMouseWorldPosition().x, -3f);
            spawnedWorker.GetComponent<TaskWorkerAI>().SetUp(spawnedWorker.GetComponent<Worker>(), taskSystem);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //TaskSystem.Task task = new TaskSystem.Task.CleanUp { targetPosition = new Vector3(4, -3), cleanUpAction =  ()=> { Debug.Log("Cleaned Up Stuff"); } };
            //taskSystem.AddTask(task);
            //taskSystem.EnqueueTaskHelper()
            SpawnMessCleanUp();

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


    private void SpawnMessCleanUp()
    {
        float cleanupTime = Time.time + 5f;
        taskSystem.EnqueueTaskHelper(() => 
        { 
            if (Time.time > cleanupTime) 
            {
                TaskSystem.Task task = new TaskSystem.Task.CleanUp { targetPosition = new Vector3(4, -3), cleanUpAction = () => { Debug.Log("Cleaned Up Stuff"); } };
                return task;
            }
            else
            {
                return null;
            }        
        });
    }
}
