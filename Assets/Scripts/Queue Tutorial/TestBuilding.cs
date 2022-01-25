using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class TestBuilding
{
    private WaitingQueue waitingQueue;
    private List<Toilet> toiletList;
    private Vector3 exitPosition;
    public TestBuilding(WaitingQueue waitingQueue, List<Vector3> toiletPositionList, Vector3 exitPosition)
    {
        this.waitingQueue = waitingQueue;
        this.exitPosition = exitPosition;

        toiletList = new List<Toilet>();

        foreach (Vector3 toiletPosition in toiletPositionList) //this is just for debug and seeing the position of toilet
        {
            toiletList.Add(new Toilet() { toiletPosition = toiletPosition });
            World_Sprite.Create(toiletPosition, Vector3.one);
        }


        waitingQueue.OnVillagerArrivedAtFrontofQueue += WaitingQueue_OnVillagerArrivedAtFront;
    }
    private void WaitingQueue_OnVillagerArrivedAtFront (object sender, System.EventArgs e)
    {
        TrySendVillagerToToilet();
    }

    private void TrySendVillagerToToilet()
    {
        Toilet emptyToilet = GetEmptyToilet();
        if(emptyToilet != null)
        {
            Villager villager = waitingQueue.GetFirstInQueue();
            if(villager != null)
            {
                emptyToilet.SetVillager(villager);  
                villager.MoveTo(emptyToilet.GetPosition(), () => { emptyToilet.ClearGuest(); villager.MoveTo(exitPosition, () => { TrySendVillagerToToilet(); });  } );
            }
        }
    }

    private Toilet GetEmptyToilet()
    {
        foreach(Toilet toilet in toiletList)
        {
            if (toilet.isEmpty())
            {
                return toilet;
            }
        }
        return null;
    }

    private class Toilet
    {
        public Villager villager;
        public Vector3 toiletPosition;

        public bool isEmpty()
        {
            return villager == null;
        }

        public void SetVillager(Villager villager)
        {
            this.villager = villager;
        }

        public Vector3 GetPosition()
        {
            return toiletPosition;
        }

        public void ClearGuest()
        {
            villager = null;
            
        }
    }

}
