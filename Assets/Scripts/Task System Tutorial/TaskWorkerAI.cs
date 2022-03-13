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
    private TaskSystem<TaskClasses.Task> taskSystem;
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

    public void SetUp(IWorker worker, TaskSystem<TaskClasses.Task> taskSystem)
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
                Roam();
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

    private void Roam()
    {
        if (Time.time > nextRoamTime)
        {
            worker.MoveTo(roamingPos);
            float reachedPosDist = 1f;
            if (Vector3.Distance(transform.position, roamingPos) < reachedPosDist)
            {
                roamingPos = GetRoamingPos();
                float chooseRate = Random.Range(.5f, .8f);
                nextRoamTime = Time.time + chooseRate;
            }

        }
    }

    private void RequestNextTask()
    {
        //Debug.Log("RequestNextTask");
        TaskClasses.Task task = taskSystem.RequestNextTask();
        if(task == null)
        {
            state = State.WaitingForNextTask;
        }
        else
        {
            state = State.ExecutingTask;
            if(task is TaskClasses.Task.MoveToPosition)
            {
                ExecuteTask_MoveToPosition(task as TaskClasses.Task.MoveToPosition);
                return;
            }
            if(task is TaskClasses.Task.Victory)
            {
                ExecuteTask_Victory(task as TaskClasses.Task.Victory);
                return;
            }
            if(task is TaskClasses.Task.CleanUp)
            {
                ExecuteTask_CleanUp(task as TaskClasses.Task.CleanUp);
                return;
            }
            if(task is TaskClasses.Task.TakeResourceToPosition)
            {
                ExecuteTask_TakeResourceToPosition(task as TaskClasses.Task.TakeResourceToPosition);
                return;
            }
            if(task is TaskClasses.Task.ConvertToTransporterTask)
            {
                ExecuteTask_ConvertTaskWorkerToTransporter(task as TaskClasses.Task.ConvertToTransporterTask);
                return;
            }


        }
    }

    private void ExecuteTask_MoveToPosition(TaskClasses.Task.MoveToPosition moveToPosTask)
    {
        Debug.Log("Execute MoveTo Task");
        worker.MoveTo(new Vector3(moveToPosTask.targetPosition.x, moveToPosTask.targetPosition.y), () => { state = State.WaitingForNextTask; });
    }

    private void ExecuteTask_Victory(TaskClasses.Task.Victory victoryTask)
    {
        Debug.Log("Execute Victory Task");
        worker.PlayAnimation(() => { Debug.Log("Finished Executing Victory Task"); state = State.WaitingForNextTask; }); 
    }
    private void ExecuteTask_CleanUp(TaskClasses.Task.CleanUp cleanUpTask)
    {
        Debug.Log("Execute CleanUp Task");
        worker.MoveTo(cleanUpTask.targetPosition, () => { cleanUpTask.cleanUpAction(); state = State.WaitingForNextTask; });

    }
    private void ExecuteTask_TakeResourceToPosition(TaskClasses.Task.TakeResourceToPosition takeResourceTask)
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
    private void ExecuteTask_ConvertTaskWorkerToTransporter(TaskClasses.Task.ConvertToTransporterTask convertTask)
    {
        Debug.Log("Execute Convert Task");
        worker.MoveTo(convertTask.buildingPosition, () => { convertTask.convertAction(this); state = State.WaitingForNextTask; });

    }
}
