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
    private TaskSystem<TaskGameHandler.Task> taskSystem;
    private State state;
    private float waitingTimer;
    private Vector3 startingPos;
    private Vector3 roamingPos;
    private float nextRoamTime;
    private Vector3 GetRandomLR()
    {
        return new Vector3(UnityEngine.Random.Range(-1, 1), 0).normalized;
    }
    private Vector3 GetRoamingPos()
    {
        return startingPos + GetRandomLR() * Random.Range(-5, 5);
    }

    public void SetUp(IWorker worker, TaskSystem<TaskGameHandler.Task> taskSystem)
    {
        this.worker = worker;
        this.taskSystem = taskSystem;
    }
    private void Start()
    {
        startingPos = FindObjectOfType<Town_Center>().transform.position;
        roamingPos = GetRoamingPos();
    }
    private void Update()
    {
        switch (state)
        {
            //worker waits to request a new task
            case State.WaitingForNextTask:
                Debug.Log("no detected task");
                if(Time.time > nextRoamTime)
                {
                    worker.MoveTo(roamingPos);
                    float reachedPosDist = 1f;
                    if (Vector3.Distance(transform.position, roamingPos) < reachedPosDist)
                    {
                        roamingPos = GetRoamingPos();
                        float chooseRate = Random.Range(.5f,.8f);
                        nextRoamTime = Time.time + chooseRate; 
                    }

                }

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
        TaskGameHandler.Task task = taskSystem.RequestNextTask();
        if(task == null)
        {
            state = State.WaitingForNextTask;
        }
        else
        {
            state = State.ExecutingTask;
            if(task is TaskGameHandler.Task.MoveToPosition)
            {
                ExecuteTask_MoveToPosition(task as TaskGameHandler.Task.MoveToPosition);
                return;
            }
            if(task is TaskGameHandler.Task.Victory)
            {
                ExecuteTask_Victory(task as TaskGameHandler.Task.Victory);
                return;
            }
            if(task is TaskGameHandler.Task.CleanUp)
            {
                ExecuteTask_CleanUp(task as TaskGameHandler.Task.CleanUp);
                return;
            }
            if(task is TaskGameHandler.Task.TakeResourceToPosition)
            {
                ExecuteTask_TakeResourceToPosition(task as TaskGameHandler.Task.TakeResourceToPosition);
                return;
            }
            if(task is TaskGameHandler.Task.ConvertToTransporterTask)
            {
                ExecuteTask_ConvertTaskWorkerToTransporter(task as TaskGameHandler.Task.ConvertToTransporterTask);
            }


        }
    }

    private void ExecuteTask_MoveToPosition(TaskGameHandler.Task.MoveToPosition moveToPosTask)
    {
        Debug.Log("Execute MoveTo Task");
        worker.MoveTo(new Vector3(moveToPosTask.targetPosition.x, moveToPosTask.targetPosition.y), () => { state = State.WaitingForNextTask; });
    }

    private void ExecuteTask_Victory(TaskGameHandler.Task.Victory victoryTask)
    {
        Debug.Log("Execute Victory Task");
        worker.PlayAnimation(() => { Debug.Log("Finished Executing Victory Task"); state = State.WaitingForNextTask; }); 
    }
    private void ExecuteTask_CleanUp(TaskGameHandler.Task.CleanUp cleanUpTask)
    {
        Debug.Log("Execute CleanUp Task");
        worker.MoveTo(cleanUpTask.targetPosition, () => { cleanUpTask.cleanUpAction(); state = State.WaitingForNextTask; });

    }
    private void ExecuteTask_TakeResourceToPosition(TaskGameHandler.Task.TakeResourceToPosition takeResourceTask)
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
    private void ExecuteTask_ConvertTaskWorkerToTransporter(TaskGameHandler.Task.ConvertToTransporterTask convertTask)
    {
        Debug.Log("Execute Convert Task");
        worker.MoveTo(convertTask.buildingPosition, () => { convertTask.convertAction(this); state = State.WaitingForNextTask; });

    }
}
