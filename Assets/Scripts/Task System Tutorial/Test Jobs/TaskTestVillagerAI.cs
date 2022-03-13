using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTestVillagerAI : MonoBehaviour
{

    private GatherWaitingQueue gatherWaitingQueue;
    private ResourceSpawner resourceSpawner;
    
    private enum State
    {
        WaitingForNextTask,
        ExecutingTask,
        TemporaryDontWaitNextTask,
    }
    private IUnit worker;
    private TaskSystem<TaskClasses.TestTaskVillager> taskSystem;
    [SerializeField] private State state;
    private float waitingTimer;
    private Vector3 startingPos;
    private Vector3 roamingPos;
    private float nextRoamTime;
    private TaskClasses.TestTaskVillager task;
    private TaskClasses.TestTaskVillager savedTestTask;
    public TaskClasses.TestTaskVillager GetSavedTestTask()
    {
        return savedTestTask;
    }
    private Vector3 GetRandomLR()
    {
        return new Vector3(UnityEngine.Random.Range(-1, 1), 0).normalized;
    }
    private Vector3 GetRoamingPos()
    {
        return startingPos + GetRandomLR() * UnityEngine.Random.Range(-5, 5);
    }

    public void SetUp(IUnit worker, TaskSystem<TaskClasses.TestTaskVillager> taskSystem)
    {
        this.worker = worker;
        this.taskSystem = taskSystem;
        
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
                float chooseRate = UnityEngine.Random.Range(.5f, .8f);
                nextRoamTime = Time.time + chooseRate;
            }

        }
    }

    private void Start()
    {
        roamingPos = GetRoamingPos();
        resourceSpawner = FindObjectOfType<ResourceSpawner>();
        gatherWaitingQueue = resourceSpawner.GetGatherWaitingQueue();
        gatherWaitingQueue.OnUnitArrivedAtFrontofQueue += GatherWaitingQueue_OnUnitArrivedAtFrontofQueue;

    }
    private void Update()
    {
        //Debug.Log("saved test task = " + GetSavedTestTask());
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
            case State.TemporaryDontWaitNextTask:
                Debug.Log("temporarily stopped doing task");
                break;
        }
    }

    public void SetBackToWaiting()
    {
        state = State.WaitingForNextTask;
    }
    public void FinishTaskEarly()
    {
        state = State.TemporaryDontWaitNextTask;
    }
    public void Directly_Do_Task(TaskClasses.TestTaskVillager directTask)
    {
        state = State.ExecutingTask;
        if(directTask is TaskClasses.TestTaskVillager.MoveToPosition)
        {
            ExecuteTask_MoveToPosition(directTask as TaskClasses.TestTaskVillager.MoveToPosition);
            return;
        }
    }

    public void DoSavedTask()
    {
        state = State.ExecutingTask;
        if (savedTestTask is TaskClasses.TestTaskVillager.MoveToPosition)
        {
            ExecuteTask_MoveToPosition(savedTestTask as TaskClasses.TestTaskVillager.MoveToPosition);
            return;
        }
        if (savedTestTask is TaskClasses.TestTaskVillager.TakeResourceFromSlotToPosition)
        {
            Debug.Log("saved test task = " + GetSavedTestTask());
            ExecuteTask_TakeResourceToPosition(savedTestTask as TaskClasses.TestTaskVillager.TakeResourceFromSlotToPosition);
            return;
        }
        if (savedTestTask is TaskClasses.TestTaskVillager.DropResourceFromPositionToSlot)
        {
            Debug.Log("saved test task = " + GetSavedTestTask());
            ExecuteTask_DropResourceToPosition(savedTestTask as TaskClasses.TestTaskVillager.DropResourceFromPositionToSlot);
            return;
        }
        if (savedTestTask is TaskClasses.TestTaskVillager.ConvertToArcher)
        {
            ExecuteTask_ConvertToArcher(savedTestTask as TaskClasses.TestTaskVillager.ConvertToArcher);
            return;
        }
        if (savedTestTask is TaskClasses.TestTaskVillager.ConvertToBuilder)
        {
            ExecuteTask_ConvertToBuilder(savedTestTask as TaskClasses.TestTaskVillager.ConvertToBuilder);
            return;
        }
        savedTestTask = null;
    }

    private void RequestNextTask()
    {
        //Debug.Log("RequestNextTask");
        task = taskSystem.RequestNextTask();
        if (task == null)
        {
            state = State.WaitingForNextTask;
            //savedTestTask = task;
        }
        else
        {
            state = State.ExecutingTask;
            if (task is TaskClasses.TestTaskVillager.MoveToPosition)
            {
                savedTestTask = task;
                Debug.Log("saved test task = " + GetSavedTestTask());
                ExecuteTask_MoveToPosition(task as TaskClasses.TestTaskVillager.MoveToPosition);
                return;
            }
            if (task is TaskClasses.TestTaskVillager.TakeResourceFromSlotToPosition)
            {
                savedTestTask = task;
                Debug.Log("saved test task = " + GetSavedTestTask());
                ExecuteTask_TakeResourceToPosition(task as TaskClasses.TestTaskVillager.TakeResourceFromSlotToPosition);
                return;
            }
            if (task is TaskClasses.TestTaskVillager.DropResourceFromPositionToSlot)
            {
                savedTestTask = task;
                Debug.Log("saved test task = " + GetSavedTestTask());
                ExecuteTask_DropResourceToPosition(task as TaskClasses.TestTaskVillager.DropResourceFromPositionToSlot);
                return;
            }
            if (task is TaskClasses.TestTaskVillager.ConvertToArcher)
            {
                savedTestTask = task;
                Debug.Log("saved test task = " + GetSavedTestTask());
                ExecuteTask_ConvertToArcher(task as TaskClasses.TestTaskVillager.ConvertToArcher);
                return;
            }
            if (task is TaskClasses.TestTaskVillager.ConvertToBuilder)
            {
                savedTestTask = task;
                Debug.Log("saved test task = " + GetSavedTestTask());
                ExecuteTask_ConvertToBuilder(task as TaskClasses.TestTaskVillager.ConvertToBuilder);
                return;
            }
        }
    }


    private void ExecuteTask_MoveToPosition(TaskClasses.TestTaskVillager.MoveToPosition moveToPosTask)
    {
        Debug.Log("Execute MoveTo Task");
        worker.MoveTo(new Vector3(moveToPosTask.targetPosition.x, moveToPosTask.targetPosition.y), () => { state = State.WaitingForNextTask; savedTestTask = null; });
    }
    private void ExecuteTask_TakeResourceToPosition(TaskClasses.TestTaskVillager.TakeResourceFromSlotToPosition takeResourceTask)//needs to integrate queue system in here
    {
        Debug.Log("Execute Take Resource To Position Task");
       
        worker.MoveTo(takeResourceTask.resourcePosition, () =>
        {
            takeResourceTask.takeResource(this);
            //start queueing up here
            resourceSpawner.DoAddGuest(GetComponent<BaseUnit>());
            //maybe add code here that when this unit gets to the resourcedepositposition do the dropresource
            /*
            worker.MoveTo(takeResourceTask.resourceDepositPosition, () => 
            {
                takeResourceTask.dropResource();
                state = State.WaitingForNextTask;
            });
            */
        });
    }
    private void ExecuteTask_DropResourceToPosition(TaskClasses.TestTaskVillager.DropResourceFromPositionToSlot dropResourceTask)//needs to integrate queue system in here
    {
        Debug.Log("Execute Drop Resource To Position Task");
       
       
        //maybe add code here that when this unit gets to the resourcedepositposition do the dropresource
        worker.MoveTo(dropResourceTask.resourceDepositPosition, () => 
        {
            dropResourceTask.dropResource();
            state = State.WaitingForNextTask;
        });
        /*
        */
        
    }

    
    private void ExecuteTask_ConvertToArcher(TaskClasses.TestTaskVillager.ConvertToArcher convertToArcherTask)
    {
        Debug.Log("Execute Convert To Archer Task");
        worker.MoveTo(convertToArcherTask.targetPosition, () => { convertToArcherTask.convertAction(this); state = State.WaitingForNextTask; });
    }
    private void ExecuteTask_ConvertToBuilder(TaskClasses.TestTaskVillager.ConvertToBuilder convertToBuilderTask)
    {
        Debug.Log("Execute Convert To Builder Task");
        worker.MoveTo(convertToBuilderTask.targetPosition, () => { convertToBuilderTask.convertAction(this); state = State.WaitingForNextTask; });
    }
    private void GatherWaitingQueue_OnUnitArrivedAtFrontofQueue(object sender, EventArgs e)
    {
        Debug.Log("Villager Arrived At Front of Queue");
    }

    

}
