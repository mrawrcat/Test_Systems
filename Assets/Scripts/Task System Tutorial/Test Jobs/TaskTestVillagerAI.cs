using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTestVillagerAI : MonoBehaviour
{
    private enum State
    {
        WaitingForNextTask,
        ExecutingTask,
        TemporaryDontWaitNextTask,
    }
    private IUnit worker;
    private TaskSystem<TaskGameHandler.TestTaskVillager> taskSystem;
    [SerializeField] private State state;
    private float waitingTimer;
    private Vector3 startingPos;
    private Vector3 roamingPos;
    private float nextRoamTime;
    private TaskGameHandler.TestTaskVillager savedTestTask;
    public TaskGameHandler.TestTaskVillager GetSavedTestTask()
    {
        return savedTestTask;
    }
    private Vector3 GetRandomLR()
    {
        return new Vector3(UnityEngine.Random.Range(-1, 1), 0).normalized;
    }
    private Vector3 GetRoamingPos()
    {
        return startingPos + GetRandomLR() * Random.Range(-5, 5);
    }

    public void SetUp(IUnit worker, TaskSystem<TaskGameHandler.TestTaskVillager> taskSystem)
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
                float chooseRate = Random.Range(.5f, .8f);
                nextRoamTime = Time.time + chooseRate;
            }

        }
    }

    private void Start()
    {
        roamingPos = GetRoamingPos();
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

    public void FinishTaskEarly()
    {
        state = State.TemporaryDontWaitNextTask;
    }
    public void SetBackToWaiting()
    {
        state = State.WaitingForNextTask;
    }

    public void DoSavedTask()
    {
        state = State.ExecutingTask;
        if (savedTestTask is TaskGameHandler.TestTaskVillager.MoveToPosition)
        {
            ExecuteTask_MoveToPosition(savedTestTask as TaskGameHandler.TestTaskVillager.MoveToPosition);
            return;
        }
        if (savedTestTask is TaskGameHandler.TestTaskVillager.TakeResourceFromSlotToPosition)
        {
            Debug.Log("saved test task = " + GetSavedTestTask());
            ExecuteTask_TakeResourceToPosition(savedTestTask as TaskGameHandler.TestTaskVillager.TakeResourceFromSlotToPosition);
            return;
        }
        if (savedTestTask is TaskGameHandler.TestTaskVillager.ConvertToArcher)
        {
            ExecuteTask_ConvertToArcher(savedTestTask as TaskGameHandler.TestTaskVillager.ConvertToArcher);
            return;
        }
        if (savedTestTask is TaskGameHandler.TestTaskVillager.ConvertToBuilder)
        {
            ExecuteTask_ConvertToBuilder(savedTestTask as TaskGameHandler.TestTaskVillager.ConvertToBuilder);
            return;
        }
        savedTestTask = null;
    }

    private void RequestNextTask()
    {
        //Debug.Log("RequestNextTask");
        TaskGameHandler.TestTaskVillager task = taskSystem.RequestNextTask();
        if (task == null)
        {
            state = State.WaitingForNextTask;
            //savedTestTask = task;
        }
        else
        {
            state = State.ExecutingTask;
            if (task is TaskGameHandler.TestTaskVillager.MoveToPosition)
            {
                savedTestTask = task;
                Debug.Log("saved test task = " + GetSavedTestTask());
                ExecuteTask_MoveToPosition(task as TaskGameHandler.TestTaskVillager.MoveToPosition);
                return;
            }
            if (task is TaskGameHandler.TestTaskVillager.TakeResourceFromSlotToPosition)
            {
                savedTestTask = task;
                Debug.Log("saved test task = " + GetSavedTestTask());
                ExecuteTask_TakeResourceToPosition(task as TaskGameHandler.TestTaskVillager.TakeResourceFromSlotToPosition);
                return;
            }
            if (task is TaskGameHandler.TestTaskVillager.ConvertToArcher)
            {
                savedTestTask = task;
                Debug.Log("saved test task = " + GetSavedTestTask());
                ExecuteTask_ConvertToArcher(task as TaskGameHandler.TestTaskVillager.ConvertToArcher);
                return;
            }
            if (task is TaskGameHandler.TestTaskVillager.ConvertToBuilder)
            {
                savedTestTask = task;
                Debug.Log("saved test task = " + GetSavedTestTask());
                ExecuteTask_ConvertToBuilder(task as TaskGameHandler.TestTaskVillager.ConvertToBuilder);
                return;
            }
            


        }
    }

    private void ExecuteTask_MoveToPosition(TaskGameHandler.TestTaskVillager.MoveToPosition moveToPosTask)
    {
        Debug.Log("Execute MoveTo Task");
        worker.MoveTo(new Vector3(moveToPosTask.targetPosition.x, moveToPosTask.targetPosition.y), () => { state = State.WaitingForNextTask; savedTestTask = null; });
    }
    private void ExecuteTask_TakeResourceToPosition(TaskGameHandler.TestTaskVillager.TakeResourceFromSlotToPosition takeResourceTask)
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
    private void ExecuteTask_ConvertToArcher(TaskGameHandler.TestTaskVillager.ConvertToArcher convertToArcherTask)
    {
        Debug.Log("Execute Convert To Archer Task");
        worker.MoveTo(convertToArcherTask.targetPosition, () => { convertToArcherTask.convertAction(this); state = State.WaitingForNextTask; });
    }
    private void ExecuteTask_ConvertToBuilder(TaskGameHandler.TestTaskVillager.ConvertToBuilder convertToBuilderTask)
    {
        Debug.Log("Execute Convert To Builder Task");
        worker.MoveTo(convertToBuilderTask.targetPosition, () => { convertToBuilderTask.convertAction(this); state = State.WaitingForNextTask; });
    }


}
