using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherWaitingQueue_AI
{
    private enum State
    {
        GoingToEntrance,
        WaitingInMiddleOfQueue,
    }
    private BaseUnit baseUnit;
    private GatherWaitingQueue gatherWaitingQueue;
    private State state;
    public GatherWaitingQueue_AI(GatherWaitingQueue gatherWaitingQueue, BaseUnit baseUnit, Vector3 entrancePosition)
    {
        this.gatherWaitingQueue = gatherWaitingQueue;
        this.baseUnit = baseUnit;
        baseUnit.SetQueuedStateQueued();
        state = State.GoingToEntrance;
        baseUnit.MoveTo(new Vector3(entrancePosition.x, 0), () =>
        {
            state = State.WaitingInMiddleOfQueue;
            gatherWaitingQueue.VillagerRequestSetQueuePosition(this);
        });
    }

    public void SetQueuePosition(Vector3 position)
    {
        baseUnit.MoveTo(new Vector3(position.x,0), () => { gatherWaitingQueue.VillagerArrivedAtQueuePosition(this); });
    }

    public BaseUnit GetUnit()
    {
        baseUnit.SetQueuedStateIdle(); //for now set here because dont have buildng to send to
        return baseUnit;
    }

    public bool IsWaitingForQueue()
    {
        return state == State.WaitingInMiddleOfQueue;
    }
}
