using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KingdomDayNightCycle : MonoBehaviour //kingdom new lands day night cycle is 4 mins
{

    private enum CycleState
    {
        DawnToSunrise,
        SunriseToNoon,
        NoonToSunset,
        SunsetToMidnight,
        MidnightToDawn,
    }

    private enum DayNight
    {
        Day,
        Night,
    }

    private CycleState cycleState;
    private float countdown = 240f; //four mins
    [SerializeField]
    private TextMeshProUGUI cycleStateText;

    // Start is called before the first frame update
    void Start()
    {
        cycleState = CycleState.DawnToSunrise;
    }

    // Update is called once per frame
    void Update()
    {

        SetCycleState();
        cycleStateText.text = cycleState.ToString();
    }

    private void SetCycleState()
    {
        countdown -= Time.deltaTime;
        if(countdown >= 220f)
        {
            cycleState = CycleState.DawnToSunrise;
        }
        else if(countdown < 220f && countdown >= 150f)
        {
            cycleState = CycleState.SunriseToNoon;
        }
        else if(countdown < 150f && countdown >= 75f)
        {
            cycleState = CycleState.NoonToSunset;
        }
        else if (countdown < 75f && countdown >= 35f)
        {
            cycleState = CycleState.SunsetToMidnight;
        }
        else if(countdown < 35f && countdown >= 0)
        {
            cycleState = CycleState.MidnightToDawn;
        }

        if(countdown <= 0)
        {
            countdown = 240f;
        }
    }
}
