using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class TaskGameHandler : MonoBehaviour
{
    


    public TaskSystem<TaskClasses.TestTaskVillager> villagerTaskSystem;
    public TaskSystem<TaskClasses.TestTask> testTaskSystem;

    [SerializeField] private Sprite PFCherrySprite;
    [SerializeField] private Sprite appleSprite;
    [SerializeField] private Sprite dewSprite;


    //private DepositSlot depositSlot;
    [SerializeField]
    private GameObject worker;
    private GameObject spawnedWorkerSave;

    [SerializeField] private Transform Archer_Station;
    private float waitingTimer;

    // Start is called before the first frame update
    private void Awake()
    {
        GameObtainableResources.Init();
        villagerTaskSystem = new TaskSystem<TaskClasses.TestTaskVillager>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    private GameObject SpawnResourcePFCherry(Vector3 position)
    {
        GameObject gameObject = new GameObject("PFCherry", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = PFCherrySprite;
        gameObject.transform.position = position;
        return gameObject;
    }
    
    private GameObject SpawnResourceApple(Vector3 position)
    {
        GameObject gameObject = new GameObject("Apple", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = appleSprite;
        gameObject.transform.position = position;
        return gameObject;
    }
    private GameObject SpawnResourceDew(Vector3 position)
    {
        GameObject gameObject = new GameObject("Dew", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = dewSprite;
        gameObject.transform.position = position;
        return gameObject;
    }

    public DepositSlot GetEmptyDepositBox(List<DepositSlot> depositSlotList)
    {
        foreach (DepositSlot depositSlot in depositSlotList)
        {
            if (depositSlot.isEmpty())
            {
                return depositSlot;
            }
        }
        return null;
    }


    public class DepositSlot
    {
        public event EventHandler OnDepositSlotEmpty;
        public BaseUnit villager;
        private Transform depositObjTransform;
        private Transform depositSlotTransform;
        private bool hasDepositIncoming;
        private GatherWaitingQueue gatherWaitingQueue;

        public DepositSlot(Transform depositSlotTransform, GatherWaitingQueue gatherWaitingQueue)
        {
            this.depositSlotTransform = depositSlotTransform;
            this.gatherWaitingQueue = gatherWaitingQueue;
            OnDepositSlotEmpty += OnDepositSlotEmpty_AddResource;
            SetDepositTransform(null);
            
        }

        public bool isEmpty()
        {
            return depositObjTransform == null && !hasDepositIncoming;
        }
        public bool isOccupied()
        {
            return villager;
        }
        public void SetVillager(BaseUnit villager)
        {
            this.villager = villager;
        }
        public void ClearVillager()
        {
            villager = null;

        }
        public void SetDepositIncoming(bool hasDepositIncoming)
        {
            this.hasDepositIncoming = hasDepositIncoming;
        }

        public void SetDepositTransform(Transform depositObjTransform)
        {
            this.depositObjTransform = depositObjTransform;
            hasDepositIncoming = false;
            UpdateSprite();
            FunctionTimer.Create(()=> 
            {
                if(depositObjTransform != null)
                {
                    Destroy(depositObjTransform.gameObject);
                    SetDepositTransform(null);
                    OnDepositSlotEmpty?.Invoke(this, EventArgs.Empty);
                }
            }, 1f);
        }

        public void OnDepositSlotEmpty_CallNextFirstInLine(object sender, EventArgs Empty)
        {
            //Debug.Log("testing call first in line");
            //BaseUnit villager = gatherWaitingQueue.GetFirstInQueue();
        }
        public void OnDepositSlotEmpty_AddResource(object sender, EventArgs Empty)
        {
            GameObtainableResources.AddResourceAmount(GameObtainableResources.ResourceType.Cherry, 1);
            //Debug.Log(GameObtainableResources.GetResourceAmount(GameObtainableResources.ResourceType.Cherry));
            //BaseUnit villager = gatherWaitingQueue.GetFirstInQueue();
        }
        

        public Vector3 GetPosition()
        {
            return depositSlotTransform.position;
        }

        public void UpdateSprite()
        {
            depositSlotTransform.GetComponent<SpriteRenderer>().color = isEmpty() ? Color.grey : Color.red;
        }
    }
}
 

