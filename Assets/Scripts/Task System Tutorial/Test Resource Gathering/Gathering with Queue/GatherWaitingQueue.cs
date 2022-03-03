using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GatherWaitingQueue : MonoBehaviour
{
    public event EventHandler OnUnitAdded;
    public event EventHandler OnUnitArrivedAtFrontofQueue;

    private List<GatherWaitingQueue_AI> gathererAIList;
    private List<Vector3> positionList;
    private Vector3 entrancePosition;
    private float gapWidth = 1.5f;
    public GatherWaitingQueue(List<Vector3> positionList)
    {
        this.positionList = positionList;
        CalculateEntrancePosition();
        /*
        foreach (Vector3 position in positionList)
        {
            World_Sprite.Create(position, new Vector3(1f, 1f), Color.green);
        }
        World_Sprite.Create(entrancePosition, new Vector3(1f, 1f), Color.magenta);
        */
        gathererAIList = new List<GatherWaitingQueue_AI>();
    }
    public bool CanAddUnit()
    {
        return gathererAIList.Count < positionList.Count;
    }


    public void AddPosition(Vector3 position)
    {
        positionList.Add(position);
        //World_Sprite.Create(position, new Vector3(1f, 1f), Color.green);
        CalculateEntrancePosition();
    }

    public void AddPositionLeft()
    {
        AddPosition(positionList[positionList.Count - 1] + new Vector3(-1, 0) * gapWidth);
    }
    public void RemovePosition()
    {
        if (gathererAIList.Count < positionList.Count)
        {
            positionList.RemoveAt(positionList.Count - 1);
            CalculateEntrancePosition();

        }
    }

    private void CalculateEntrancePosition()
    {
        if (positionList.Count < 1)
        {
            entrancePosition = positionList[positionList.Count - 1] + new Vector3(gapWidth, 0);
            //World_Sprite.Create(entrancePosition, new Vector3(1f, 1f), Color.magenta);
        }
        else
        {
            Vector3 dir = positionList[positionList.Count - 1] - positionList[positionList.Count - 2];
            entrancePosition = positionList[positionList.Count - 1] + dir;
            //World_Sprite.Create(entrancePosition, new Vector3(1f, 1f), Color.magenta);
        }
    }
    public void UnitRequestSetQueuePosition(GatherWaitingQueue_AI villagerAI)
    {
        for (int i = 0; i < gathererAIList.Count; i++)
        {
            if (gathererAIList[i] == villagerAI)
            {
                break;
            }
            else
            {
                if (!gathererAIList[i].IsWaitingForQueue())
                {
                    gathererAIList[gathererAIList.IndexOf(villagerAI)] = gathererAIList[i];
                    gathererAIList[i] = villagerAI;
                    break;
                }
            }
        }
        villagerAI.SetQueuePosition(positionList[gathererAIList.IndexOf(villagerAI)]);
    }

    public void AddUnit(BaseUnit villagerObj)//cant check if villager is in queue now because dont know how to check if contains
    {
        if (!villagerObj.IsQueued())
        {
            GatherWaitingQueue_AI villagerAI = new GatherWaitingQueue_AI(this, villagerObj, entrancePosition);
            gathererAIList.Add(villagerAI);
        }
        else
        {
            Debug.Log("villager already in Queue");
        }
        Debug.Log(gathererAIList.Count);

        OnUnitAdded?.Invoke(this, EventArgs.Empty);


        //villagerAI.SetQueuePosition(positionList[villagerAIList.IndexOf(villagerAI)]);
        //villagerObj.MoveTo(entrancePosition, () => { villagerObj.MoveTo(positionList[villagerAIList.IndexOf(villagerObj)], () => { VillagerArrivedAtQueuePosition(villagerObj); }); });
    }

    public BaseUnit GetFirstInQueue()
    {
        if (gathererAIList.Count == 0)
        {
            return null;
        }
        else
        {
            BaseUnit baseUnit = gathererAIList[0].GetUnit();
            gathererAIList.RemoveAt(0);
            //villager.SetQueuedStateIdle();//for now set to idle here because dont have building to send to
            RelocateAllRalliedVillagers();
            return baseUnit;
        }
    }


    private void RelocateAllRalliedVillagers()
    {

        for (int i = 0; i < gathererAIList.Count; i++)
        {
            GatherWaitingQueue_AI baseUnitAI = gathererAIList[i];
            if (baseUnitAI.IsWaitingForQueue())
            {
                baseUnitAI.SetQueuePosition(positionList[gathererAIList.IndexOf(baseUnitAI)]);
            }
            //villager.MoveTo(positionList[i], () => { VillagerArrivedAtQueuePosition(villager); } );
        }

    }

    public void UnitArrivedAtQueuePosition(GatherWaitingQueue_AI villagerAI)
    {

        if (villagerAI == gathererAIList[0])
        {
            OnUnitArrivedAtFrontofQueue?.Invoke(this, EventArgs.Empty);
        }

    }

    public int GetCurrentListCount()
    {
        return gathererAIList.Count;
    }

}
