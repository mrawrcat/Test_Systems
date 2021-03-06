using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Enemy_Spawner : MonoBehaviour
{
    private List<BaseEnemy> allEnemyList = new List<BaseEnemy>();
    //private TaskSystem<TaskGameHandler.Task_IEnemy_Unit> enemyUnitTaskSystem;
    //private GameObject spawnEnemySave;
    //private Vector3 savedTargetPos;
    // Start is called before the first frame update
    void Start()
    {
        //allEnemyList = new List<BaseEnemy>();
        
        /*
        enemyUnitTaskSystem = new TaskSystem<TaskGameHandler.Task_IEnemy_Unit>();
        spawnedEnemy.GetComponent<Enemy_Spearman_AI>().SetUp(spawnedEnemy.GetComponent<BaseEnemy>(), enemyUnitTaskSystem);
        spawnEnemySave = spawnedEnemy;
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
           BaseEnemy.Create_BaseUnit(transform.position, new Vector3(-3, -3));
        }
        /*
        if (Input.GetKeyDown(KeyCode.B))
        {
            //TaskGameHandler.Task_IEnemy_Unit task = new TaskGameHandler.Task_IEnemy_Unit.MoveToPosition { targetPosition = new Vector3(-30f, -3f) };
            //enemyUnitTaskSystem.AddTask(task);

            //GetHitStopMovingThenContinueMoving(savedTargetPos);
            spawnEnemySave.GetComponent<Enemy_Spearman_AI>().SetStateWaiting();
            spawnEnemySave.GetComponent<BaseEnemy>().MoveTo(spawnEnemySave.transform.position);
            //spawnEnemySave.GetComponent<TestStateAnimation>().SetStateHurt();
            FunctionTimer.Create(() =>
            {
                //spawnEnemySave.GetComponent<TestStateAnimation>().SetStateRun();
                //spawnEnemySave.GetComponent<BaseEnemy>().MoveTo(savedTargetPos);
                TaskGameHandler.Task_IEnemy_Unit task = new TaskGameHandler.Task_IEnemy_Unit.MoveToPosition { targetPosition = savedTargetPos };
                enemyUnitTaskSystem.AddTask(task);
            }, .3f);

        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            savedTargetPos = new Vector3(30f, -3f);
            TaskGameHandler.Task_IEnemy_Unit task = new TaskGameHandler.Task_IEnemy_Unit.MoveToPosition { targetPosition = savedTargetPos };
            enemyUnitTaskSystem.AddTask(task);
            
        }
        */
    }
    /*
    private void GetHitStopMovingThenContinueMoving(Vector3 targetPos)
    {
        float stopTime = .3f;
        TaskGameHandler.Task_IEnemy_Unit task = new TaskGameHandler.Task_IEnemy_Unit.GetHitThenContinueToTarget
        {
            stopAction = (Enemy_Spearman_AI spearman_AI) =>
            {
                //spearman_AI.GetComponent<TestStateAnimation>().SetStateHurt();
                FunctionTimer.Create(() =>
                {
                    //spearman_AI.GetComponent<TestStateAnimation>().SetStateRun();
                    spearman_AI.GetComponent<BaseEnemy>().MoveTo(targetPos);
                }, stopTime);
            },
        };
    }
    */
}
