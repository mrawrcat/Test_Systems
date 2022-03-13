using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class TestTaskClass : MonoBehaviour
{
    [SerializeField] private Sprite PFCherrySprite;
    [SerializeField] private Sprite DepositSlotSprite;
    public TaskSystem<TaskClasses.TestTaskVillager> villagerTaskSystem;
    [SerializeField] private int numOfUnits;
    // Start is called before the first frame update
    void Start()
    {
        villagerTaskSystem = new TaskSystem<TaskClasses.TestTaskVillager>();
        for(int i = 0; i < numOfUnits; i++)
        {
            BaseUnit.Create_BaseUnit(transform.position + new Vector3(-10 + (1.5f* i), 0), transform.position, BaseUnit.UnitType.Villager);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SendTask(new Vector3(UtilsClass.GetMouseWorldPosition().x, 0));
        }
    }

    private void SendTask(Vector3 resourcePos)
    {
        /*
        TaskClasses.TestTaskVillager gatherTask = new TaskClasses.TestTaskVillager.MoveToPosition
        {
            targetPosition = new Vector3(20, -3)
        };
        */
        GameObject resourceGameObject = SpawnResourcePFCherry(resourcePos);
        villagerTaskSystem.EnqueueTaskHelper(() =>
        {
            TaskClasses.TestTaskVillager gatherTask = new TaskClasses.TestTaskVillager.TakeResourceFromSlotToPosition
            {
                resourcePosition = resourceGameObject.transform.position,
                takeResource = (TaskTestVillagerAI villagerAI) =>
                {
                    resourceGameObject.transform.SetParent(villagerAI.transform);
                    resourceGameObject.transform.position = villagerAI.transform.position + new Vector3(0, 2);
                },
            };

            return gatherTask;
        });
    }

    private GameObject SpawnResourcePFCherry(Vector3 position) //resources need a collider so that resource spawner can detect them
    {
        GameObject gameObject = new GameObject("PFCherry", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = PFCherrySprite;
        gameObject.layer = LayerMask.NameToLayer("Resource");
        gameObject.AddComponent<ResourceObject>();
        gameObject.AddComponent<CircleCollider2D>();
        gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
        gameObject.transform.position = position;
        return gameObject;
    }
}
