using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;


public class GatherWaitingQueueDeposit
{
    //building logic for gathering queue
    private GatherWaitingQueue gatherWaitingQueue;
    private List<TaskGameHandler.DepositSlot> depositBoxList;
    private Vector3 exitPosition;
    private ResourceSpawner resourceSpawner;

    public GatherWaitingQueueDeposit(GatherWaitingQueue gatherWaitingQueue, List<Vector3> depositBoxPosList, Vector3 exitPosition, ResourceSpawner spawnerReference)
    {
        this.gatherWaitingQueue = gatherWaitingQueue;
        this.exitPosition = exitPosition;
        resourceSpawner = spawnerReference;
        depositBoxList = new List<TaskGameHandler.DepositSlot>();
        
        foreach (Vector3 depositBoxPosition in depositBoxPosList) //this is just for debug and seeing the position of toilet
        {
            //depositBoxList.Add(new TaskGameHandler.DepositSlot() { depositBoxPos = depositBoxPosition });
            //World_Sprite.Create(depositBoxPosition, Vector3.one);
        }


        gatherWaitingQueue.OnUnitArrivedAtFrontofQueue += WaitingQueue_OnVillagerArrivedAtFront;
        resourceSpawner.GetResourceDepositSlot().OnDepositSlotEmpty += OnDepositBoxEmpty_GetFirstInLine;
    }


    private void WaitingQueue_OnVillagerArrivedAtFront(object sender, System.EventArgs e)
    {
        TaskGameHandler.DepositSlot depositSlot = resourceSpawner.GetResourceDepositSlot();
        Debug.Log("deposit slot is empty?: " + resourceSpawner.GetResourceDepositSlot().isEmpty());
        Debug.Log("deposit slot is occupied?: " + resourceSpawner.GetResourceDepositSlot().isOccupied());
        Debug.Log(GetEmptyDepositSlot());
        if(depositSlot.isEmpty() && !depositSlot.isOccupied())
        {
            TrySendVillager();
        }
        //TrySendVillagerToDepositBox();
    }

    

    private void TrySendVillager()
    {
        TaskGameHandler.DepositSlot depositBox = resourceSpawner.GetResourceDepositSlot();
        BaseUnit villager = gatherWaitingQueue.GetFirstInQueue();
        if(villager != null)
        {
            resourceSpawner.GetResourceDepositSlot().SetDepositIncoming(true);
            depositBox.SetVillager(villager);

            villager.MoveTo(new Vector3(depositBox.GetPosition().x, 0), () =>
            {
                //need to drop resource here

                Transform resourceObjTransform = villager.transform.Find("PFCherry").transform;
                TaskClasses.TestTaskVillager depositTask = new TaskClasses.TestTaskVillager.DropResourceFromPositionToSlot
                {
                    resourcePosition = resourceObjTransform.position,
                    resourceDepositPosition = depositBox.GetPosition(),
                    dropResource = () =>
                    {
                        resourceObjTransform.position = depositBox.GetPosition();
                        resourceSpawner.GetResourceDepositSlot().SetDepositTransform(resourceObjTransform);
                        resourceObjTransform.SetParent(null);
                    },
                };
                villager.GetComponent<TaskTestVillagerAI>().Directly_Do_Task(depositTask);

                depositBox.ClearVillager();
                villager.MoveTo(new Vector3(exitPosition.x, 0), () =>
                {
                    villager.GetComponent<TaskTestVillagerAI>().SetBackToWaiting();
                });
            
            });
        }


    }

    private void TrySendVillagerToDepositBox()
    {
        TaskGameHandler.DepositSlot depositBox = resourceSpawner.GetResourceDepositSlot();
        if (depositBox != null)
        {
            BaseUnit villager = gatherWaitingQueue.GetFirstInQueue();
            if (resourceSpawner.GetResourceDepositSlot().isEmpty() && !resourceSpawner.GetResourceDepositSlot().isOccupied())
            {
                if (villager != null)
                {

                    resourceSpawner.GetResourceDepositSlot().SetDepositIncoming(true);
                    depositBox.SetVillager(villager);

                    villager.MoveTo(new Vector3(depositBox.GetPosition().x, 0), () =>
                    {
                        //need to drop resource here

                        Transform resourceObjTransform = villager.transform.Find("PFCherry").transform;
                        TaskClasses.TestTaskVillager depositTask = new TaskClasses.TestTaskVillager.DropResourceFromPositionToSlot
                        {
                            resourcePosition = resourceObjTransform.position,
                            resourceDepositPosition = depositBox.GetPosition(),
                            dropResource = () =>
                            {
                                resourceObjTransform.position = depositBox.GetPosition();
                                resourceSpawner.GetResourceDepositSlot().SetDepositTransform(resourceObjTransform);
                                resourceObjTransform.SetParent(null);
                            },
                        };
                        villager.GetComponent<TaskTestVillagerAI>().Directly_Do_Task(depositTask);

                        depositBox.ClearVillager();
                        villager.MoveTo(exitPosition, () => 
                        { 
                            TrySendVillagerToDepositBox();//this is the last one, if the deposit slot isnt empty by the time this villager gets to exit position the first in queue wont receive any commands
                            villager.GetComponent<TaskTestVillagerAI>().SetBackToWaiting();
                        }); 
                        /*
                        */
                    });
                }
            }
            
        }
    }

    private TaskGameHandler.DepositSlot GetEmptyDepositSlot()
    {
        foreach(TaskGameHandler.DepositSlot depositBox in depositBoxList)
        {
            if (depositBox.isEmpty())
            {
                return depositBox;
            }
            
        }
        return null;
    }

    private void OnDepositBoxEmpty_GetFirstInLine(object sender, EventArgs eventArgs)
    {
        Debug.Log("box finished deposing of resource, get first in line to do something");
        TrySendVillager();
    }

    /*
    private DepositBox GetEmptyDepositBox()
    {
        foreach (DepositBox depositBox in depositBoxList)
        {
            if (depositBox.isEmpty())
            {
                return depositBox;
            }
        }
        return null;
    }
    public class DepositBox
    {
        public BaseUnit villager;
        public Vector3 depositPosition;

        public bool isEmpty()
        {
            return villager == null;
        }

        public void SetVillager(BaseUnit villager)
        {
            this.villager = villager;
        }

        public Vector3 GetPosition()
        {
            return depositPosition;
        }

        public void ClearGuest()
        {
            villager = null;

        }
    }
    */
}
