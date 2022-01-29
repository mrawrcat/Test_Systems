using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitQueue_FollowerAI
{
    private enum State
    {
        GoingToEntrance,
        WaitingInMiddleOfQueue,
    }

    private enum IsfollowerQueued
    {
        InQueue,
        Not_InQueue,
    }
    private Follower follower;
    private OrbitQueue orbitQueue;
    private State state;
    public OrbitQueue_FollowerAI(OrbitQueue orbitQueue, Follower follower, Transform entrancePosition)
    {
        this.orbitQueue = orbitQueue;
        this.follower = follower;
        follower.SetQueuedStateQueued();
        state = State.GoingToEntrance;
        follower.MoveToTransform(entrancePosition, () =>
        {
            state = State.WaitingInMiddleOfQueue;
            orbitQueue.VillagerRequestSetQueuePosition(this);
        });
    }

    public void SetQueuePosition(Transform position)
    {
        follower.MoveToTransform(position, () => { orbitQueue.VillagerArrivedAtQueuePosition(this); });
    }

    public Follower GetFollower()
    {
        follower.SetQueuedStateIdle(); //for now set here because dont have buildng to send to
        return follower;
    }

    public bool IsWaitingForQueue()
    {
        return state == State.WaitingInMiddleOfQueue;
    }
}
