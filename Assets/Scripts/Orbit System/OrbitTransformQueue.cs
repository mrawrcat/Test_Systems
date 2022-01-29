using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class OrbitTransformQueue
{
    public event EventHandler OnFollowerAdded;
    public event EventHandler OnFollowerArrivedAtFrontofQueue;
    private List<Follower> followerList;
    private List<Transform> positionList;
    private Vector3 entrancePosition;
    //private Transform bleh;
    public OrbitTransformQueue(List<Transform> positionList)
    {
        this.positionList = positionList;
        //bleh.position = positionList[positionList.Count - 1].position + new Vector3(-1.5f, 0);

        //CalculateEntrancePosition();
        /*
        foreach (Transform position in positionList)
        {
            //should be create some queue debug squares but cant find a way to do it without tex and stuff
            World_Sprite.Create(position.position, new Vector3(1f, 1f), Color.green);
        }
        */
        //World_Sprite.Create(bleh.position, new Vector3(1f, 1f), Color.magenta);

        followerList = new List<Follower>();
    }

    public bool canAddFollower()
    {
        return followerList.Count < positionList.Count;
    }

    public void AddFollower(Follower followerObj)
    {
        if (!followerList.Contains(followerObj))
        {
            followerList.Add(followerObj);
            //followerObj.MoveToTransform(bleh, () => { followerObj.MoveToTransform(positionList[followerList.IndexOf(followerObj)], () => { followerArrivedAtQueuePosition(followerObj); }); });
            followerObj.MoveToTransform(positionList[followerList.IndexOf(followerObj)], () => { FollowerArrivedAtQueuePosition(followerObj); });
            if (OnFollowerAdded != null)
            {
                OnFollowerAdded(this, EventArgs.Empty);
            }

        }
        else
        {
            Debug.Log("follower already added to list");
        }
        Debug.Log(followerList.Count);
    }

    public Follower GetFirstInQueue()
    {
        if (followerList.Count == 0)
        {
            return null;
        }
        else
        {
            Follower follower = followerList[0];
            followerList.RemoveAt(0);
            RelocateAllRalliedfollowers();
            return follower;
        }
    }
    private void CalculateEntrancePosition() //this calculates entrance position which we dont have yet
    {
        if (positionList.Count < 1)
        {
            entrancePosition = positionList[positionList.Count - 1].position + new Vector3(-1.5f, 0);
            //bleh.position = entrancePosition;
        }
        else
        {
            Vector3 dir = positionList[positionList.Count - 1].position - positionList[positionList.Count - 2].position;
            entrancePosition = positionList[positionList.Count - 1].position + dir;
            //bleh.position = entrancePosition;
        }
    }

    public void AddPosition(Transform position)
    {
        positionList.Add(position);
        World_Sprite.Create(position.position, new Vector3(1f, 1f), Color.green);
        CalculateEntrancePosition();
    }

    private void RelocateAllRalliedfollowers()
    {
        for (int i = 0; i < followerList.Count; i++)
        {
            Follower follower = followerList[i];
            follower.MoveToTransform(positionList[i], () => { FollowerArrivedAtQueuePosition(follower); });
        }
        //Debug.Log(followerList.Count);
    }

    private void FollowerArrivedAtQueuePosition(Follower follower)
    {
        if (follower == followerList[0])
        {
            if (OnFollowerArrivedAtFrontofQueue != null)
            {
                OnFollowerArrivedAtFrontofQueue(this, EventArgs.Empty);
            }
            //Debug.Log("Arrived At front");
        }
    }
}
