using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCommand : MonoBehaviour
{

    
    [SerializeField] private LayerMask whatIsHobo;
    [SerializeField] private Vector2 detectSize;
    private TaskGameHandler taskGameHandler;
    // Start is called before the first frame update
    void Start()
    {
        taskGameHandler = FindObjectOfType<TaskGameHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            //if have enough resource do makehobovillager
            MakeHoboToVillager();
        }
        
    }

    private void MakeHoboToVillager()
    {
        Collider2D[] hobos = Physics2D.OverlapBoxAll(transform.position, detectSize, 0, whatIsHobo);
        foreach(Collider2D hobo in hobos)
        {
            TaskTestHoboAI hoboAI = hobo.GetComponent<TaskTestHoboAI>();
            if(hoboAI != null)
            {
                if(hoboAI.enabled == true)
                {
                    TaskTestVillagerAI villagerAI = hobo.GetComponent<TaskTestVillagerAI>();
                    villagerAI.enabled = true;
                    hobo.GetComponent<TaskTestHoboAI>().enabled = false;
                    villagerAI.FinishTaskEarly();
                    hobo.GetComponent<BaseUnit>().MoveTo(new Vector3(-5, -3), ()=> { villagerAI.SetBackToWaiting(); });
                    //inject the task to villager -> make him move to keep -> then he should roam until he gets new task
                    Debug.Log("found first Hobo, make him villager, subtract resource");
                    return;
                }
            }
            /*
            TaskTestVillagerAI villagerAI = hobo.GetComponent<TaskTestVillagerAI>();
            TaskTestNewWorkerAI workerAI = hobo.GetComponent<TaskTestNewWorkerAI>();
            if(villagerAI.enabled == false && workerAI.enabled == false)
            {
                villagerAI.enabled = true;
                TaskGameHandler.TestTaskVillager task = new TaskGameHandler.TestTaskVillager.MoveToPosition 
                { 
                    targetPosition = new Vector3(0, -3)
                };
                taskGameHandler.villagerTaskSystem.AddTask(task);
                
                Debug.Log("Found 1 hobo, debug should only be 1");
            }*/

        }
    }
}
