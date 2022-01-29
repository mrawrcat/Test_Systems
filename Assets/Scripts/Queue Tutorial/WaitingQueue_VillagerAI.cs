using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class WaitingQueue_VillagerAI 
{
    private enum State
    {
        GoingToEntrance,
        WaitingInMiddleOfQueue,
    }

    private enum IsVillagerQueued
    {
        InQueue,
        Not_InQueue,
    }
    private Villager villager;
    private WaitingQueue waitingQueue;
    private State state;
    public WaitingQueue_VillagerAI(WaitingQueue waitingQueue, Villager villager, Vector3 entrancePosition)
    {
        this.waitingQueue = waitingQueue;
        this.villager = villager;
        villager.SetQueuedStateQueued();
        state = State.GoingToEntrance;
        villager.MoveTo(entrancePosition, () =>
        {
            state = State.WaitingInMiddleOfQueue;
            waitingQueue.VillagerRequestSetQueuePosition(this); 
        });
    }

    public void SetQueuePosition(Vector3 position)
    {
        villager.MoveTo(position, () => { waitingQueue.VillagerArrivedAtQueuePosition(this); });
    }

    public Villager GetVillager()
    {
        villager.SetQueuedStateIdle(); //for now set here because dont have buildng to send to
        return villager;
    }

    public bool IsWaitingForQueue()
    {
        return state == State.WaitingInMiddleOfQueue;
    }
   

}
