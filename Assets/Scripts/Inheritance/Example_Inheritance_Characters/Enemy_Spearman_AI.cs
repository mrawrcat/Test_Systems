using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spearman_AI : MonoBehaviour
{
    private enum State
    {
        WaitingForNextTask,
        ExecutingTask,
    }
    private IEnemy_Unit enemy;
    private TaskSystem<TaskClasses.Task_IEnemy_Unit> taskSystem;
    private State state;
    private float waitingTimer;
    private Vector3 startingPos;
    private TestStateAnimation testStateAnim;
    private Vector3 GetRandomLR()
    {
        return new Vector3(UnityEngine.Random.Range(-1, 1), 0).normalized;
    }
    private Vector3 GetRoamingPos()
    {
        return startingPos + GetRandomLR() * Random.Range(-5, 5);
    }

    public void SetUp(IEnemy_Unit enemy, TaskSystem<TaskClasses.Task_IEnemy_Unit> taskSystem)
    {
        this.enemy = enemy;
        this.taskSystem = taskSystem;
    }
    
    public void Start()
    {

        //startingPos = FindObjectOfType<Town_Center>().transform.position;
        startingPos = transform.position;
        testStateAnim = GetComponent<TestStateAnimation>();
        //roamingPos = GetRoamingPos();

    }
    
    private void Update()
    {
        switch (state)
        {
            //enemy waits to request a new task, maybe assign when spawn from spawner
            case State.WaitingForNextTask:
                Debug.Log("no detected task");
                waitingTimer -= Time.deltaTime;
                if (waitingTimer <= 0)
                {
                    float waitingTimerMax = .2f;
                    waitingTimer = waitingTimerMax;
                    RequestNextTask();
                }
                break;
            case State.ExecutingTask:
                Debug.Log("trying to do task");
                break;
        }
    }

    private void RequestNextTask()
    {
        TaskClasses.Task_IEnemy_Unit task = taskSystem.RequestNextTask();
        if (task == null)
        {
            state = State.WaitingForNextTask;
        }
        else
        {
            state = State.ExecutingTask;
            if (task is TaskClasses.Task_IEnemy_Unit.MoveToPosition)
            {
                ExecuteTask_MoveToPosition(task as TaskClasses.Task_IEnemy_Unit.MoveToPosition);
                return;
            }
            if (task is TaskClasses.Task_IEnemy_Unit.Victory)
            {
                ExecuteTask_Victory(task as TaskClasses.Task_IEnemy_Unit.Victory);
                return;
            }
            if (task is TaskClasses.Task_IEnemy_Unit.CleanUp)
            {
                ExecuteTask_CleanUp(task as TaskClasses.Task_IEnemy_Unit.CleanUp);
                return;
            }
            if (task is TaskClasses.Task_IEnemy_Unit.TakeResourceToPosition)
            {
                ExecuteTask_TakeResourceToPosition(task as TaskClasses.Task_IEnemy_Unit.TakeResourceToPosition);
                return;
            }
            if (task is TaskClasses.Task_IEnemy_Unit.ConvertToTransporterTask)
            {
                ExecuteTask_ConvertTaskWorkerToTransporter(task as TaskClasses.Task_IEnemy_Unit.ConvertToTransporterTask);
            } 
            if (task is TaskClasses.Task_IEnemy_Unit.GetHitThenContinueToTarget)
            {
                ExecuteTask_GetHitThenContinueToTarget(task as TaskClasses.Task_IEnemy_Unit.GetHitThenContinueToTarget);
            }
           

        }
    }

    private void ExecuteTask_MoveToPosition(TaskClasses.Task_IEnemy_Unit.MoveToPosition moveToPosTask)
    {
        Debug.Log("Execute MoveTo Task");
        //testStateAnim.SetStateRun(); 
        enemy.MoveTo(new Vector3(moveToPosTask.targetPosition.x, moveToPosTask.targetPosition.y), () => { state = State.WaitingForNextTask; /*testStateAnim.SetStateIdle(); */});
    }

    private void ExecuteTask_Victory(TaskClasses.Task_IEnemy_Unit.Victory victoryTask)
    {
        Debug.Log("Execute Victory Task");
        //enemy.PlayAnimation(() => { Debug.Log("Finished Executing Victory Task"); state = State.WaitingForNextTask; });
    }
    private void ExecuteTask_CleanUp(TaskClasses.Task_IEnemy_Unit.CleanUp cleanUpTask)
    {
        Debug.Log("Execute CleanUp Task");
        enemy.MoveTo(cleanUpTask.targetPosition, () => { cleanUpTask.cleanUpAction(); state = State.WaitingForNextTask; });

    }
    
    private void ExecuteTask_TakeResourceToPosition(TaskClasses.Task_IEnemy_Unit.TakeResourceToPosition takeResourceTask)
    {
        Debug.Log("Execute Take Resource To Position Task");
        enemy.MoveTo(takeResourceTask.resourcePosition, () =>
        {
            takeResourceTask.takeResource(this);
            enemy.MoveTo(takeResourceTask.resourceDepositPosition, () =>
            {
                takeResourceTask.dropResource();
                state = State.WaitingForNextTask;
            });
        });

    }
    private void ExecuteTask_ConvertTaskWorkerToTransporter(TaskClasses.Task_IEnemy_Unit.ConvertToTransporterTask convertTask)
    {
        Debug.Log("Execute Convert Task");
        enemy.MoveTo(convertTask.buildingPosition, () => { convertTask.convertAction(this); state = State.WaitingForNextTask; });

    }
    private void ExecuteTask_GetHitThenContinueToTarget(TaskClasses.Task_IEnemy_Unit.GetHitThenContinueToTarget stopStartTask)
    {
        Debug.Log("Execute stop continue Task");
        //testStateAnim.SetStateHurt();
        enemy.MoveTo(transform.position, () => { stopStartTask.stopAction(this); state = State.WaitingForNextTask; });

    }
    
    public void SetStateWaiting()//maybe this is interupt task
    {
        state = State.WaitingForNextTask;
    }
}
