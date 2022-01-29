using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class OrbitQueue
{
    public event EventHandler OnVillagerAdded;
    public event EventHandler OnVillagerArrivedAtFrontofQueue;
    private List<OrbitQueue_FollowerAI> villagerAIList;
    private List<Transform> positionList;
    private Transform entrancePosition;
    public OrbitQueue(List<Transform> positionList)
    {
        this.positionList = positionList;
        CalculateEntrancePosition();
        villagerAIList = new List<OrbitQueue_FollowerAI>();
    }
    public bool canAddVillager()
    {
        return villagerAIList.Count < positionList.Count;
    }


    public void AddPosition(Transform position)
    {
        positionList.Add(position);
        World_Sprite.Create(position.position, new Vector3(1f, 1f), Color.green);
        CalculateEntrancePosition();
    }
    /*
    public void AddPositionLeft()
    {
        AddPosition(positionList[positionList.Count - 1] + new Vector3(-1, 0) * 1.5f);
    }
    */
    public void RemovePosition()
    {
        if (villagerAIList.Count < positionList.Count)
        {
            positionList.RemoveAt(positionList.Count - 1);
            CalculateEntrancePosition();

        }
    }

    private void CalculateEntrancePosition()
    {
        if (positionList.Count < 1)
        {
            entrancePosition = positionList[positionList.Count - 1];
            World_Sprite.Create(entrancePosition.position, new Vector3(1f, 1f), Color.magenta);
        }
        else
        {
            /*
            Vector3 dir = positionList[positionList.Count - 1] - positionList[positionList.Count - 2];
            entrancePosition = positionList[positionList.Count - 1] + dir;
            World_Sprite.Create(entrancePosition, new Vector3(1f, 1f), Color.magenta);
            */
        }
    }
    public void VillagerRequestSetQueuePosition(OrbitQueue_FollowerAI villagerAI)
    {
        for (int i = 0; i < villagerAIList.Count; i++)
        {
            if (villagerAIList[i] == villagerAI)
            {
                break;
            }
            else
            {
                if (!villagerAIList[i].IsWaitingForQueue())
                {
                    villagerAIList[villagerAIList.IndexOf(villagerAI)] = villagerAIList[i];
                    villagerAIList[i] = villagerAI;
                    break;
                }
            }
        }
        villagerAI.SetQueuePosition(positionList[villagerAIList.IndexOf(villagerAI)]);
    }

    public void AddVillager(Follower villagerObj)//cant check if villager is in queue now because dont know how to check if contains
    {
        if (!villagerObj.IsQueued())
        {
            OrbitQueue_FollowerAI villagerAI = new OrbitQueue_FollowerAI(this, villagerObj, entrancePosition);
            villagerAIList.Add(villagerAI);
        }
        else
        {
            Debug.Log("villager already in Queue");
        }
        Debug.Log(villagerAIList.Count);



        //villagerAI.SetQueuePosition(positionList[villagerAIList.IndexOf(villagerAI)]);
        //villagerObj.MoveTo(entrancePosition, () => { villagerObj.MoveTo(positionList[villagerAIList.IndexOf(villagerObj)], () => { VillagerArrivedAtQueuePosition(villagerObj); }); });
        if (OnVillagerAdded != null)
        {
            OnVillagerAdded(this, EventArgs.Empty);
        }


    }

    public Follower GetFirstInQueue()
    {
        if (villagerAIList.Count == 0)
        {
            return null;
        }
        else
        {
            Follower villager = villagerAIList[0].GetFollower();
            villagerAIList.RemoveAt(0);
            //villager.SetQueuedStateIdle();//for now set to idle here because dont have building to send to
            RelocateAllRalliedVillagers();
            return villager;
        }
    }


    private void RelocateAllRalliedVillagers()
    {

        for (int i = 0; i < villagerAIList.Count; i++)
        {
            OrbitQueue_FollowerAI villagerAI = villagerAIList[i];
            if (villagerAI.IsWaitingForQueue())
            {
                villagerAI.SetQueuePosition(positionList[villagerAIList.IndexOf(villagerAI)]);
            }
            //villager.MoveTo(positionList[i], () => { VillagerArrivedAtQueuePosition(villager); } );
        }

    }

    public void VillagerArrivedAtQueuePosition(OrbitQueue_FollowerAI villagerAI)
    {

        if (villagerAI == villagerAIList[0])
        {
            if (OnVillagerArrivedAtFrontofQueue != null)
            {
                OnVillagerArrivedAtFrontofQueue(this, EventArgs.Empty);
            }
            //Debug.Log("Arrived At front");
        }

    }

    public int GetCurrentListCount()
    {
        return villagerAIList.Count;
    }
}
