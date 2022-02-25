using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTestHoboAI : MonoBehaviour
{
    private enum State
    {
        WaitingForNextTask,
        ExecutingTask,
        TemporaryDontWaitNextTask,
    }
    private IUnit worker;
    private TaskSystem<TaskGameHandler.TestTaskHobo> taskSystem;
    [SerializeField] private State state;
    private float waitingTimer;
    [SerializeField] private Vector3 startingPos;
    private Vector3 roamingPos;
    private float nextRoamTime;
    private TaskGameHandler.TestTaskHobo savedTestTask;
    public TaskGameHandler.TestTaskHobo GetSavedTestTask()
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

    public void SetUp(IUnit worker, TaskSystem<TaskGameHandler.TestTaskHobo> taskSystem, Vector3 startingRoamPos)
    {
        this.worker = worker;
        this.taskSystem = taskSystem;
        startingPos = startingRoamPos;
        nextRoamTime = 0;
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
                float chooseRate = Random.Range(1f, 1.5f);
                nextRoamTime = Time.time + chooseRate;
            }

        }
    }

    private void Start()
    {
        //startingPos = new Vector3(0, -3);
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
                //Roam();
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

    public void DoSavedTask()
    {
        state = State.ExecutingTask;
        if (savedTestTask is TaskGameHandler.TestTaskHobo.MoveToPosition)
        {
            ExecuteTask_MoveToPosition(savedTestTask as TaskGameHandler.TestTaskHobo.MoveToPosition);
            return;
        }
        if (savedTestTask is TaskGameHandler.TestTaskHobo.ConvertToVillager)
        {
            ExecuteTask_ConvertToVillager(savedTestTask as TaskGameHandler.TestTaskHobo.ConvertToVillager);
            return;
        }
        savedTestTask = null;
    }

    private void RequestNextTask()
    {
        //Debug.Log("RequestNextTask");
        TaskGameHandler.TestTaskHobo task = taskSystem.RequestNextTask();
        if (task == null)
        {
            state = State.WaitingForNextTask;
            //savedTestTask = task;
        }
        else
        {
            state = State.ExecutingTask;
            if (task is TaskGameHandler.TestTaskHobo.MoveToPosition)
            {
                savedTestTask = task;
                Debug.Log("saved test task = " + GetSavedTestTask());
                ExecuteTask_MoveToPosition(task as TaskGameHandler.TestTaskHobo.MoveToPosition);
                return;
            }
            if (task is TaskGameHandler.TestTaskHobo.ConvertToVillager)
            {
                savedTestTask = task;
                Debug.Log("saved test task = " + GetSavedTestTask());
                ExecuteTask_ConvertToVillager(task as TaskGameHandler.TestTaskHobo.ConvertToVillager);
                return;
            }

        }
    }

    private void ExecuteTask_MoveToPosition(TaskGameHandler.TestTaskHobo.MoveToPosition moveToPosTask)
    {
        Debug.Log("Execute MoveTo Task");
        worker.MoveTo(new Vector3(moveToPosTask.targetPosition.x, moveToPosTask.targetPosition.y), () => { state = State.WaitingForNextTask; savedTestTask = null; });
    }

    private void ExecuteTask_ConvertToVillager(TaskGameHandler.TestTaskHobo.ConvertToVillager convertToVillagerTask)
    {
        Debug.Log("Execute Convert To Villager Task");
        worker.MoveTo(convertToVillagerTask.targetPosition, () => { convertToVillagerTask.convertAction(); state = State.WaitingForNextTask; });
    }

}
