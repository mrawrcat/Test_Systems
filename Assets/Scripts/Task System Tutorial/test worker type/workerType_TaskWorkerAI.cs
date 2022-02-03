using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class workerType_TaskWorkerAI : MonoBehaviour
{
    private enum State
    {
        WaitingForNextTask,
        ExecutingTask,
    }
    private IWorker worker;
    private workerType_TaskSystem taskSystem;
    private State state;
    private float waitingTimer;

    
    public void SetUp(IWorker worker, workerType_TaskSystem taskSystem)
    {
        this.worker = worker;
        this.taskSystem = taskSystem;
    }

    private void Update()
    {
        switch (state)
        {
            //worker waits to request a new task
            case State.WaitingForNextTask:
                Debug.Log("no detected task");
                waitingTimer -= Time.deltaTime;
                if(waitingTimer <= 0)
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
        //Debug.Log("RequestNextTask");
        workerType_TaskSystem.Task task = taskSystem.RequestNextTask();
        if(task == null)
        {
            state = State.WaitingForNextTask;
        }
        else
        {
            state = State.ExecutingTask;
            if(task is workerType_TaskSystem.Task.MoveToPosition)
            {
                ExecuteTask_MoveToPosition(task as workerType_TaskSystem.Task.MoveToPosition);
                return;
            }
            if(task is workerType_TaskSystem.Task.Victory)
            {
                ExecuteTask_Victory(task as workerType_TaskSystem.Task.Victory);
                return;
            }
            if(task is workerType_TaskSystem.Task.CleanUp)
            {
                ExecuteTask_CleanUp(task as workerType_TaskSystem.Task.CleanUp);
                return;
            }
            if(task is workerType_TaskSystem.Task.TakeResourceToPosition)
            {
                ExecuteTask_TakeResourceToPosition(task as workerType_TaskSystem.Task.TakeResourceToPosition);
                return;
            }


        }
    }

    private void ExecuteTask_MoveToPosition(workerType_TaskSystem.Task.MoveToPosition moveToPosTask)
    {
        Debug.Log("Execute MoveTo Task");
        worker.MoveTo(new Vector3(moveToPosTask.targetPosition.x, moveToPosTask.targetPosition.y), () => { state = State.WaitingForNextTask; });
    }

    private void ExecuteTask_Victory(workerType_TaskSystem.Task.Victory victoryTask)
    {
        Debug.Log("Execute Victory Task");
        worker.PlayAnimation(() => { Debug.Log("Finished Executing Victory Task"); state = State.WaitingForNextTask; }); 
    }
    private void ExecuteTask_CleanUp(workerType_TaskSystem.Task.CleanUp cleanUpTask)
    {
        Debug.Log("Execute CleanUp Task");
        worker.MoveTo(cleanUpTask.targetPosition, () => { cleanUpTask.cleanUpAction(); state = State.WaitingForNextTask; });

    }
    private void ExecuteTask_TakeResourceToPosition(workerType_TaskSystem.Task.TakeResourceToPosition takeResourceTask)
    {
        Debug.Log("Execute Take Resource To Position Task");
        worker.MoveTo(takeResourceTask.resourcePosition, () => 
        { 
            takeResourceTask.takeResource(this); 
            worker.MoveTo(takeResourceTask.resourceDepositPosition, () => 
            { 
                takeResourceTask.dropResource(); 
                state = State.WaitingForNextTask; 
            }); 
        });

    }
}
