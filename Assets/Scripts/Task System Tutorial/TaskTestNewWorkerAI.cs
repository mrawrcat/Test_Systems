using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTestNewWorkerAI : MonoBehaviour
{
    private enum State
    {
        WaitingForNextTask,
        ExecutingTask,
        TemporaryDontWaitNextTask,
    }
    private IUnit worker;
    private TaskSystem<TaskClasses.TestTask> taskSystem;
    [SerializeField] private State state;
    private float waitingTimer;
    private TaskClasses.TestTask savedTestTask;
    public TaskClasses.TestTask GetSavedTestTask()
    {
        return savedTestTask;
    }


    public void SetUp(IUnit worker, TaskSystem<TaskClasses.TestTask> taskSystem)
    {
        this.worker = worker;
        this.taskSystem = taskSystem;
    }

    private void Update()
    {
        //Debug.Log("saved test task = " + GetSavedTestTask());
        switch (state)
        {
            //worker waits to request a new task
            case State.WaitingForNextTask:
                //Debug.Log("no detected task");
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
        if (savedTestTask is TaskClasses.TestTask.MoveToPosition)
        {
            ExecuteTask_MoveToPosition(savedTestTask as TaskClasses.TestTask.MoveToPosition);
            return;
        }
        if (savedTestTask is TaskClasses.TestTask.MoveToPositionThenDie)
        {
            ExecuteTask_MoveToPositionThenDie(savedTestTask as TaskClasses.TestTask.MoveToPositionThenDie);
            return;
        }
        if (savedTestTask is TaskClasses.TestTask.StopAndAttack)
        {
            ExecuteTask_StopAndAttack(savedTestTask as TaskClasses.TestTask.StopAndAttack);
            return;
        }
        savedTestTask = null;
    }

    private void RequestNextTask()
    {
        //Debug.Log("RequestNextTask");
        TaskClasses.TestTask task = taskSystem.RequestNextTask();
        if (task == null)
        {
            state = State.WaitingForNextTask;
            //savedTestTask = task;
        }
        else
        {
            state = State.ExecutingTask;
            if (task is TaskClasses.TestTask.MoveToPosition)
            {
                savedTestTask = task;
                Debug.Log("saved test task = " + GetSavedTestTask());
                ExecuteTask_MoveToPosition(task as TaskClasses.TestTask.MoveToPosition);
                return;
            }
            if (task is TaskClasses.TestTask.MoveToPositionThenDie)
            {
                savedTestTask = task;
                Debug.Log("saved test task = " + GetSavedTestTask());
                ExecuteTask_MoveToPositionThenDie(task as TaskClasses.TestTask.MoveToPositionThenDie);
                return;
            }
            if (task is TaskClasses.TestTask.StopAndAttack)
            {
                savedTestTask = task;
                Debug.Log("saved test task = " + GetSavedTestTask());
                ExecuteTask_StopAndAttack(task as TaskClasses.TestTask.StopAndAttack);
                return;
            }


        }
    }

    private void ExecuteTask_MoveToPosition(TaskClasses.TestTask.MoveToPosition moveToPosTask)
    {
        Debug.Log("Execute MoveTo Task");
        worker.MoveTo(new Vector3(moveToPosTask.targetPosition.x, moveToPosTask.targetPosition.y), () => { state = State.WaitingForNextTask; savedTestTask = null; });
    }
    private void ExecuteTask_MoveToPositionThenDie(TaskClasses.TestTask.MoveToPositionThenDie moveToPosThenDieTask)
    {
        Debug.Log("Execute MoveToAndDie Task");
        worker.MoveTo(new Vector3(moveToPosThenDieTask.targetPosition.x, moveToPosThenDieTask.targetPosition.y), () => { moveToPosThenDieTask.DieAction(this); state = State.WaitingForNextTask; savedTestTask = null; });
    } 
    private void ExecuteTask_StopAndAttack(TaskClasses.TestTask.StopAndAttack stopAndAttackTask)
    {
        Debug.Log("Execute Stop And Attack Task");
        worker.MoveTo(transform.position, () => { stopAndAttackTask.AttackAction(this); state = State.WaitingForNextTask; savedTestTask = null; });
    }

    
}
