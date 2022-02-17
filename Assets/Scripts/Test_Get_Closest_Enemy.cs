using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class Test_Get_Closest_Enemy : MonoBehaviour
{
    [SerializeField] private List<GameObject> testobsList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for(int i =0; i< 5; i++)
        {
            GameObject newTestObj = new GameObject();
            newTestObj.transform.SetParent(transform);
            testobsList.Add(newTestObj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameObject randomObj = transform.GetChild(Random.Range(0, transform.childCount)).gameObject;
            Debug.Log("this is sibling #" + randomObj.transform.GetSiblingIndex() + ". It's position in the list is #" + testobsList.IndexOf(randomObj));
            testobsList.RemoveAt(testobsList.IndexOf(randomObj));
            Debug.Log("this is sibling #" + randomObj.transform.GetSiblingIndex() + ". It's position in the list is #" + testobsList.IndexOf(randomObj));

            //testobsList.RemoveAt(0);
            //need to remove the one you click on or something and then make it tell you the index of the one you removed
            /*
            transform.position = UtilsClass.GetMouseWorldPosition();
            BaseEnemy baseEnemy = BaseEnemy.GetClosestEnemy(transform.position, 3f);
            if(baseEnemy != null)
            {
                Debug.Log(baseEnemy.transform.position);
                CMDebug.TextPopup("testing popup", transform.position);
            }
            */
        }
    }
}
