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
    private TaskSystem<TaskClasses.GhostTask> taskSystem;
    private State state;
    private float waitingTimer;


    public void SetUp(IWorker worker, TaskSystem<TaskClasses.GhostTask> taskSystem)
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
        TaskClasses.GhostTask task = taskSystem.RequestNextTask();
        if (task == null)
        {
            state = State.WaitingForNextTask;
        }
        else
        {
            state = State.ExecutingTask;
            if (task is TaskClasses.GhostTask.MoveToPosition)
            {
                ExecuteTask_MoveToPosition(task as TaskClasses.GhostTask.MoveToPosition);
                return;
            }
        }
    }

    private void ExecuteTask_MoveToPosition(TaskClasses.GhostTask.MoveToPosition moveToPosTask)
    {
        Debug.Log("Execute MoveTo Task");
        worker.MoveTo(new Vector3(moveToPosTask.targetPosition.x, moveToPosTask.targetPosition.y), () => { state = State.WaitingForNextTask; });
    }

}
