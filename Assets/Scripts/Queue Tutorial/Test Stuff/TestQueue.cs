using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class TestQueue
{
    public event EventHandler OnVillagerAdded;
    public event EventHandler OnVillagerArrivedAtFrontofQueue;
    private List<Villager> villagerList;
    private List<Transform> positionList;
    private Vector3 entrancePosition;
    //private Transform bleh;
    public TestQueue (List<Transform> positionList)
    {
        this.positionList = positionList;
        //bleh.position = positionList[positionList.Count - 1].position + new Vector3(-1.5f, 0);
       
        //CalculateEntrancePosition();
        foreach (Transform position in positionList)
        {
            //should be create some queue debug squares but cant find a way to do it without tex and stuff
            World_Sprite.Create(position.position, new Vector3(1f, 1f), Color.green);
        }
        //World_Sprite.Create(bleh.position, new Vector3(1f, 1f), Color.magenta);

        //entrancePosition = bleh.transform.position;
        villagerList = new List<Villager>();
    }

    public bool canAddVillager()
    {
        return villagerList.Count < positionList.Count;
    }

    public void AddVillager(Villager villagerObj)
    {
        if (!villagerList.Contains(villagerObj))
        {
            villagerList.Add(villagerObj);
            //villagerObj.MoveToTransform(bleh, () => { villagerObj.MoveToTransform(positionList[villagerList.IndexOf(villagerObj)], () => { VillagerArrivedAtQueuePosition(villagerObj); }); });
            villagerObj.MoveToTransform(positionList[villagerList.IndexOf(villagerObj)], () => { VillagerArrivedAtQueuePosition(villagerObj); });
            if (OnVillagerAdded != null)
            {
                OnVillagerAdded(this, EventArgs.Empty);
            }

        }
        else
        {
            Debug.Log("villager already added to list");
        }
        Debug.Log(villagerList.Count);
    }

    public Villager GetFirstInQueue()
    {
        if (villagerList.Count == 0)
        {
            return null;
        }
        else
        {
            Villager villager = villagerList[0];
            villagerList.RemoveAt(0);
            RelocateAllRalliedVillagers();
            return villager;
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
    public void VillagerFollow()
    {

    }

    private void RelocateAllRalliedVillagers()
    {
        for (int i = 0; i < villagerList.Count; i++)
        {
            Villager villager = villagerList[i];
            villager.MoveToTransform(positionList[i], () => { VillagerArrivedAtQueuePosition(villager); });
        }
        //Debug.Log(villagerList.Count);
    }

    private void VillagerArrivedAtQueuePosition(Villager villager)
    {
        if (villager == villagerList[0])
        {
            if (OnVillagerArrivedAtFrontofQueue != null)
            {
                OnVillagerArrivedAtFrontofQueue(this, EventArgs.Empty);
            }
            //Debug.Log("Arrived At front");
        }
    }
}
