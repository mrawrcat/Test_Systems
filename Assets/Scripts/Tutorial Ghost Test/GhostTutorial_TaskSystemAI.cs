using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTutorial_TaskSystemAI : MonoBehaviour
{
    private enum State
    {
        WaitingForNextTask,
        ExecutingTask,
    }
    private IWorker worker;
    private TaskSystem<TaskGameHandler.GhostTask> taskSystem;
    private State state;
    private float waitingTimer;


    public void SetUp(IWorker worker, TaskSystem<TaskGameHandler.GhostTask> taskSystem)
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
                Debug.Log("ghost no detected task");
                waitingTimer -= Time.deltaTime;
                if (waitingTimer <= 0)
                {
                    float waitingTimerMax = .2f;
                    waitingTimer = waitingTimerMax;
                    RequestNextTask();
                }
                break;
            case State.ExecutingTask:
                Debug.Log("ghost trying to do task");
                break;
        }
    }

    private void RequestNextTask()
    {
        //Debug.Log("RequestNextTask");
        TaskGameHandler.GhostTask task = taskSystem.RequestNextTask();
        if (task == null)
        {
            state = State.WaitingForNextTask;
        }
        else
        {
            state = State.ExecutingTask;
            if (task is TaskGameHandler.GhostTask.MoveToPosition)
            {
                ExecuteTask_MoveToPosition(task as TaskGameHandler.GhostTask.MoveToPosition);
                return;
            }
        }
    }

    private void ExecuteTask_MoveToPosition(TaskGameHandler.GhostTask.MoveToPosition moveToPosTask)
    {
        Debug.Log("Execute MoveTo Task");
        worker.MoveTo(new Vector3(moveToPosTask.targetPosition.x, moveToPosTask.targetPosition.y), () => { state = State.WaitingForNextTask; });
    }

}
