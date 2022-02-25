using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTestHoboAI : MonoBehaviour
{
    private BaseUnitDecoupleState decoupleState;
    private BaseUnit baseUnit;
    private void Start()
    {
        baseUnit = GetComponent<BaseUnit>();
        decoupleState = GetComponent<BaseUnitDecoupleState>();
        //startingPos = transform.position;
        nextRoamTime = 0;
        roamingPos = GetRoamingPos();
    }

    public void SetUp(Vector3 startRoamingPos)
    {
        SetStartPos(startRoamingPos);
    }

    [SerializeField] private Vector3 startingPos;
    private Vector3 roamingPos;
    private float nextRoamTime;
    public void SetStartPos(Vector3 startRoamPos)
    {
        startingPos = startRoamPos;
    }
    private Vector3 GetRandomLR()
    {
        return new Vector3(UnityEngine.Random.Range(-1, 1), 0).normalized;
    }
    private Vector3 GetRoamingPos()
    {
        return startingPos + GetRandomLR() * UnityEngine.Random.Range(-5, 5);
    }
    private void Roam()
    {
        if (Time.time > nextRoamTime)
        {
            baseUnit.MoveTo(roamingPos);
            float reachedPosDist = 1f;
            if (Vector3.Distance(transform.position, roamingPos) < reachedPosDist)
            {
                roamingPos = GetRoamingPos();
                float chooseRate = UnityEngine.Random.Range(1f, 1.5f);
                nextRoamTime = Time.time + chooseRate;
            }

        }
    }
}
