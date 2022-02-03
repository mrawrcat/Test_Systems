using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public abstract class TaskBase
{
    
}
public class QueuedTask<TTask> where TTask : TaskBase
{
    private Func<TTask> tryGetTaskFunc;
    public QueuedTask(Func<TTask> tryGetTaskFunc)
    {
        this.tryGetTaskFunc = tryGetTaskFunc;
    }

    public TTask TryDequeueTasks()
    {
        return tryGetTaskFunc();
    }
}
public class TaskSystem<TTask> where TTask : TaskBase
{
    
    private List<TTask> taskList;
    private List<QueuedTask<TTask>> queuedTaskList; //any queued task must be validated before being dequeued
    public TaskSystem()
    {
        taskList = new List<TTask>();
        queuedTaskList = new List<QueuedTask<TTask>>();
        FunctionPeriodic.Create(DequeueTasks, .2f);
    }

    public TTask RequestNextTask()
    {
        if(taskList.Count > 0)
        {
            //give worker first task
            TTask task = taskList[0];
            taskList.RemoveAt(0);
            return task;
        }
        else //no task available
        {
            return null;
        }
    }

    public void AddTask(TTask task)
    {
        taskList.Add(task);
    }

    public void EnqueueTask(QueuedTask<TTask> queuedTask)
    {
        queuedTaskList.Add(queuedTask);

    }

    public void EnqueueTaskHelper(Func<TTask> tryGetTaskFunc)
    {
        QueuedTask<TTask> queuedTask = new QueuedTask<TTask>(tryGetTaskFunc);
        queuedTaskList.Add(queuedTask);
    }

    private void DequeueTasks()
    {
        for(int i = 0; i < queuedTaskList.Count; i++)
        {
            QueuedTask<TTask> queuedTask = queuedTaskList[i];
            TTask task = queuedTask.TryDequeueTasks();
            if(task != null)
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


