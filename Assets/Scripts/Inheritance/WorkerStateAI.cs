using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class WorkerStateAI : MonoBehaviour
{

    private Vector3 GetRandomLR()
    {
        return new Vector3(UnityEngine.Random.Range(-1, 1), 0).normalized;
    }
    private enum StateAI
    {
        Roaming,
        Attacking,
        Cowering,
    }
    private StateAI stateAI;
    private Vector3 startingPos;
    private Vector3 roamingPos;
    private Worker worker;
    // Start is called before the first frame update
    void Start()
    {
        worker = GetComponent<Worker>();
        startingPos = FindObjectOfType<Town_Center>().transform.position;
        roamingPos = GetRoamingPos();
    }

    // Update is called once per frame
    void Update()
    {
        

        switch (stateAI)
        {
            default:
            case StateAI.Roaming:
                worker.MoveTo(roamingPos);
                float reachedPosDist = 1f;
                if (Vector3.Distance(transform.position, roamingPos) < reachedPosDist)
                {
                    roamingPos = GetRoamingPos();
                }
                break;
            case StateAI.Attacking:

                break;
        }
    }

    private Vector3 GetRoamingPos()
    {
        return startingPos + GetRandomLR() * Random.Range(3, 10);
    }
}
