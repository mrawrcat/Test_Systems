using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEmptyVillagerPool : MonoBehaviour
{

    [SerializeField]
    private GameObject prefab;
    private List<GameObject> villagerObjList;
    private TaskSystem taskSystem;
    // Start is called before the first frame update
    void Start()
    {
        villagerObjList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddObj()
    {
        GameObject villagerObj = Instantiate(prefab);
        villagerObj.GetComponent<TaskWorkerAI>().SetUp(villagerObj.GetComponent<Worker>(), taskSystem);
        villagerObj.transform.parent = gameObject.transform;
        villagerObj.SetActive(false);
        villagerObjList.Add(villagerObj);
    }

    public GameObject GetVillagerObj()
    {
        for(int i = 0; i< villagerObjList.Count; i++)
        {
            if (!villagerObjList[i].activeInHierarchy)
            {
                return villagerObjList[i];
            }
        }

        GameObject villagerObj = Instantiate(prefab);
        villagerObj.SetActive(false);
        villagerObjList.Add(villagerObj);
        return villagerObj;
    }

    public void GetVillagerAndSetActive(Vector3 pos)
    {
        GameObject blue = GetVillagerObj();
        blue.transform.position = pos;
        blue.SetActive(true);
    }
}
