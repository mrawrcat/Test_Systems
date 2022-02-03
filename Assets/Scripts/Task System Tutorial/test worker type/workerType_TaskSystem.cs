using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class workerType_TaskSystem
{
    public class QueuedTask
    {
        private Func<Task> tryGetTaskFunc;
        public QueuedTask(Func<Task> tryGetTaskFunc)
        {
            this.tryGetTaskFunc = tryGetTaskFunc;
        }

        public Task TryDequeueTasks()
        {
            return tryGetTaskFunc();
        }
    }

    public abstract class Task
    {


        public class MoveToPosition : Task
        {
            public Vector3 targetPosition;

        }

        public class Victory : Task
        {

        }

        public class CleanUp : Task
        {
            public Vector3 targetPosition;
            public Action cleanUpAction;
        }

        public class TakeResourceToPosition : Task //grabs a resource and takes it to building? position
        {
            public Vector3 resourcePosition;
            public Action<workerType_TaskWorkerAI> takeResource;
            public Vector3 resourceDepositPosition; //position where worker deposits resource
            public Action dropResource;
        }
    }

    private List<Task> taskList;
    private List<QueuedTask> queuedTaskList; //any queued task must be validated before being dequeued
    public workerType_TaskSystem()
    {
        taskList = new List<Task>();
        queuedTaskList = new List<QueuedTask>();
        FunctionPeriodic.Create(DequeueTasks, .2f);
    }

    public Task RequestNextTask()
    {
        if (taskList.Count > 0)
        {
            //give worker first task
            Task task = taskList[0];
            taskList.RemoveAt(0);
            return task;
        }
        else //no task available
        {
            return null;
        }
    }

    public void AddTask(Task task)
    {
        taskList.Add(task);
    }

    private void EnqueueTask(QueuedTask queuedTask)
    {
        queuedTaskList.Add(queuedTask);

    }

    public void EnqueueTaskHelper(Func<Task> tryGetTaskFunc)
    {
        QueuedTask queuedTask = new QueuedTask(tryGetTaskFunc);
        queuedTaskList.Add(queuedTask);
    }

    private void DequeueTasks()
    {
        for (int i = 0; i < queuedTaskList.Count; i++)
        {
            QueuedTask queuedTask = queuedTaskList[i];
            Task task = queuedTask.TryDequeueTasks();
            if (task != null)
            {
                AddTask(task);
                queuedTaskList.RemoveAt(i);
                i--;
            }
            else
            {
                //returned task is null keep it queued
            }
        }
    }
}
