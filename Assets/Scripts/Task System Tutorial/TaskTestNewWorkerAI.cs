using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTestNewWorkerAI : MonoBehaviour
{
    private enum State
    {
        WaitingForNextTask,
        ExecutingTask,
    }
    private IUnit worker;
    private TaskSystem<TaskGameHandler.TestTask> taskSystem;
    private State state;
    private float waitingTimer;


    public void SetUp(IUnit worker, TaskSystem<TaskGameHandler.TestTask> taskSystem)
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
        //Debug.Log("RequestNextTask");
        TaskGameHandler.TestTask task = taskSystem.RequestNextTask();
        if (task == null)
        {
            state = State.WaitingForNextTask;
        }
        else
        {
            state = State.ExecutingTask;
            if (task is TaskGameHandler.TestTask.MoveToPosition)
            {
                ExecuteTask_MoveToPosition(task as TaskGameHandler.TestTask.MoveToPosition);
                return;
            }
            if (task is TaskGameHandler.TestTask.MoveToPositionThenDie)
            {
                ExecuteTask_MoveToPositionThenDie(task as TaskGameHandler.TestTask.MoveToPositionThenDie);
                return;
            }


        }
    }

    private void ExecuteTask_MoveToPosition(TaskGameHandler.TestTask.MoveToPosition moveToPosTask)
    {
        Debug.Log("Execute MoveTo Task");
        worker.MoveTo(new Vector3(moveToPosTask.targetPosition.x, moveToPosTask.targetPosition.y), () => { state = State.WaitingForNextTask; });
    }
    private void ExecuteTask_MoveToPositionThenDie(TaskGameHandler.TestTask.MoveToPositionThenDie moveToPosThenDieTask)
    {
        Debug.Log("Execute MoveTo Task");
        worker.MoveTo(new Vector3(moveToPosThenDieTask.targetPosition.x, moveToPosThenDieTask.targetPosition.y), () => { moveToPosThenDieTask.DieAction(this); state = State.WaitingForNextTask; });
    }

    
}
