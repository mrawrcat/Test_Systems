using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class WaitingQueue
{
    public event EventHandler OnVillagerAdded;
    public event EventHandler OnVillagerArrivedAtFrontofQueue;
    private List<WaitingQueue_VillagerAI> villagerAIList;
    private List<Vector3> positionList;
    private Vector3 entrancePosition;
    public WaitingQueue(List<Vector3> positionList)
    {
        this.positionList = positionList;
        CalculateEntrancePosition();
        foreach (Vector3 position in positionList)
        {
            //should be create some queue debug squares but cant find a way to do it without tex and stuff
            World_Sprite.Create(position, new Vector3(1f, 1f), Color.green);
        }
        World_Sprite.Create(entrancePosition, new Vector3(1f, 1f), Color.magenta);

        villagerAIList = new List<WaitingQueue_VillagerAI>();
    }
    public bool canAddVillager()
    {
        return villagerAIList.Count < positionList.Count;
    }
    

    public void AddPosition(Vector3 position)
    {
        positionList.Add(position);
        World_Sprite.Create(position, new Vector3(1f, 1f), Color.green);
        CalculateEntrancePosition();
    }

    public void AddPositionLeft()
    {
        AddPosition(positionList[positionList.Count - 1] + new Vector3(-1 , 0) * 1.5f );
    }
    public void RemovePosition()
    {
        if(villagerAIList.Count < positionList.Count)
        {
            positionList.RemoveAt(positionList.Count - 1);
            CalculateEntrancePosition();

        }
    }

    private void CalculateEntrancePosition()
    {
        if (positionList.Count < 1)
        {
            entrancePosition = positionList[positionList.Count - 1] + new Vector3(-1.5f, 0);
            World_Sprite.Create(entrancePosition, new Vector3(1f, 1f), Color.magenta);
        }
        else
        {
            Vector3 dir = positionList[positionList.Count - 1] - positionList[positionList.Count - 2];
            entrancePosition = positionList[positionList.Count - 1] + dir;
            World_Sprite.Create(entrancePosition, new Vector3(1f, 1f), Color.magenta);
        }
    }
    public void VillagerRequestSetQueuePosition(WaitingQueue_VillagerAI villagerAI)
    {
        for(int i = 0; i< villagerAIList.Count; i++)
        {
            if(villagerAIList[i] == villagerAI)
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

    public void AddVillager(Villager villagerObj)//cant check if villager is in queue now because dont know how to check if contains
    {
        if (!villagerObj.IsQueued())
        {
            WaitingQueue_VillagerAI villagerAI = new WaitingQueue_VillagerAI(this, villagerObj, entrancePosition);
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

    public Villager GetFirstInQueue()
    {
        if(villagerAIList.Count == 0)
        {
            return null;
        }
        else
        {           
            Villager villager = villagerAIList[0].GetVillager();
            villagerAIList.RemoveAt(0);
            //villager.SetQueuedStateIdle();//for now set to idle here because dont have building to send to
            RelocateAllRalliedVillagers();
            return villager;
        }
    }
    

    private void RelocateAllRalliedVillagers()
    {
        
        for(int i = 0; i < villagerAIList.Count; i++)
        {
            WaitingQueue_VillagerAI villagerAI = villagerAIList[i];
            if (villagerAI.IsWaitingForQueue())
            {
                villagerAI.SetQueuePosition(positionList[villagerAIList.IndexOf(villagerAI)]);
            }
            //villager.MoveTo(positionList[i], () => { VillagerArrivedAtQueuePosition(villager); } );
        }
        
    }

    public void VillagerArrivedAtQueuePosition(WaitingQueue_VillagerAI villagerAI)
    {
        
        if(villagerAI == villagerAIList[0])
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
