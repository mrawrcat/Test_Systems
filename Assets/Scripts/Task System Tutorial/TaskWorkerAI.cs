using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskWorkerAI : MonoBehaviour
{
    private enum State
    {
        WaitingForNextTask,
        ExecutingTask,
    }
    private IWorker worker;
    private TaskSystem taskSystem;
    private State state;
    private float waitingTimer;

    
    public void SetUp(IWorker worker, TaskSystem taskSystem)
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
        TaskSystem.Task task = taskSystem.RequestNextTask();
        if(task == null)
        {
            state = State.WaitingForNextTask;
        }
        else
        {
            state = State.ExecutingTask;
            if(task is TaskSystem.Task.MoveToPosition)
            {
                ExecuteTask_MoveToPosition(task as TaskSystem.Task.MoveToPosition);
                return;
            }
            if(task is TaskSystem.Task.Victory)
            {
                ExecuteTask_Victory(task as TaskSystem.Task.Victory);
                return;
            }
            if(task is TaskSystem.Task.CleanUp)
            {
                ExecuteTask_CleanUp(task as TaskSystem.Task.CleanUp);
                return;
            }


        }
    }

    private void ExecuteTask_MoveToPosition(TaskSystem.Task.MoveToPosition moveToPosTask)
    {
        Debug.Log("Execute MoveTo Task");
        worker.MoveTo(new Vector3(moveToPosTask.targetPosition.x, -3f), () => { state = State.WaitingForNextTask; });
    }

    private void ExecuteTask_Victory(TaskSystem.Task.Victory victoryTask)
    {
        Debug.Log("Execute Victory Task");
        worker.PlayAnimation(() => { Debug.Log("Finished Executing Victory Task"); state = State.WaitingForNextTask; }); 
    }
    private void ExecuteTask_CleanUp(TaskSystem.Task.CleanUp cleanUpTask)
    {
        Debug.Log("Execute CleanUp Task");
        worker.MoveTo(cleanUpTask.targetPosition, () => { cleanUpTask.cleanUpAction(); state = State.WaitingForNextTask; });

    }
}
