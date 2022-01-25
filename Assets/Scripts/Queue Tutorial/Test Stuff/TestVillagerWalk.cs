using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVillagerWalk : MonoBehaviour
{

    private Villager villager;
    // Start is called before the first frame update
    void Start()
    {
        villager = GetComponent<Villager>();

        /*
        villager.MoveTo(new Vector3(6, -3f), () =>
        {
            Debug.Log("reached first pos");
            villager.MoveTo(new Vector3(-6, -3f), () =>
            {
                villager.MoveTo(new Vector3(6, -3f));
            });
        });
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            villager.MoveTo(new Vector3(0, -3f));
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            villager.MoveTo(new Vector3(6, -3f), () =>
            {
                Debug.Log("reached first pos");
                villager.MoveTo(new Vector3(-6, -3f), () =>
                {
                    villager.MoveTo(new Vector3(6, -3f));
                });
            });
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            villager.setCurrentPos(new Vector3(2, -3f));
        }
    }

    
}
