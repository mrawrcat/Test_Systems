using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerCommand : MonoBehaviour
{

    private LayerMask whatIsHobo;
    [SerializeField] private Vector2 detectSize;
    private TaskGameHandler taskGameHandler;
    // Start is called before the first frame update
    void Start()
    {
        taskGameHandler = FindObjectOfType<TaskGameHandler>();
        whatIsHobo = LayerMask.GetMask("Villager");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            //if have enough resource do makehobovillager
            MakeHoboToVillager();
        }

    }

    private void MakeHoboToVillager()
    {
        Collider2D[] hobos = Physics2D.OverlapBoxAll(transform.position, detectSize, 0, whatIsHobo);
        foreach (Collider2D hobo in hobos)
        {
            TaskTestHoboAI hoboAI = hobo.GetComponent<TaskTestHoboAI>();
            TaskTestVillagerAI villagerAI = hobo.GetComponent<TaskTestVillagerAI>();
            if (hoboAI != null)
            {
                if (hoboAI.enabled == true)
                {
                    hobo.GetComponent<BaseUnit>().MoveTo(new Vector3(-10, -3));
                    villagerAI.enabled = true;
                    //villagerAI.FinishTaskEarly();
                    //inject the task to villager -> make him move to keep -> then he should roam until he gets new task
                    Debug.Log("found first Hobo, make him villager, subtract resource");
                    hobo.GetComponent<TaskTestHoboAI>().enabled = false;
                }
            }
            //villagerAI.GetComponent<BaseUnit>().MoveTo(new Vector3(-10, -3), () => { villagerAI.SetBackToWaiting(); });
            TaskGameHandler.TestTaskVillager directTask = new TaskGameHandler.TestTaskVillager.MoveToPosition
            {
                targetPosition = new Vector3(-10,-3)
            };
            villagerAI.Directly_Do_Task(directTask);
            return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, detectSize);
    }
}
