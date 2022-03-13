using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter_TaskWorkerAI : MonoBehaviour
{
    private enum State
    {
        WaitingForNextTask,
        ExecutingTask,
    }
    private IWorker worker;
    private TaskSystem<TaskClasses.TransporterTask> taskSystem;
    private State state;
    private float waitingTimer;

    
    public void SetUp(IWorker worker, TaskSystem<TaskClasses.TransporterTask> taskSystem)
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
        TaskClasses.TransporterTask task = taskSystem.RequestNextTask();
        if(task == null)
        {
            state = State.WaitingForNextTask;
        }
        else
        {
            state = State.ExecutingTask;
            if (task is TaskClasses.TransporterTask.MoveToPosition)
            {
                ExecuteTask_MoveToPosition(task as TaskClasses.TransporterTask.MoveToPosition);
                return;
            }
            if (task is TaskClasses.TransporterTask.TakeWeaponFromSlotToPosition)
            {
                ExecuteTask_TakeResourceToPosition(task as TaskClasses.TransporterTask.TakeWeaponFromSlotToPosition);
                return;
            }


        }
    }

    private void ExecuteTask_MoveToPosition(TaskClasses.TransporterTask.MoveToPosition moveToPosTask)
    {
        Debug.Log("Execute MoveTo Task");
        worker.MoveTo(new Vector3(moveToPosTask.targetPosition.x, moveToPosTask.targetPosition.y), () => { state = State.WaitingForNextTask; });
    }

    private void ExecuteTask_TakeResourceToPosition(TaskClasses.TransporterTask.TakeWeaponFromSlotToPosition takeResourceTask)
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
