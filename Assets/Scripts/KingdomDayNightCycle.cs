using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KingdomDayNightCycle : MonoBehaviour //kingdom new lands day night cycle is 4 mins
{
    public event EventHandler OnStateChanged;
    public event EventHandler OnDawnStart;
    public event EventHandler OnNightStart;

    public enum CycleState
    {
        DawnToSunrise,
        SunriseToNoon,
        NoonToSunset,
        SunsetToMidnight,
        MidnightToDawn,
    }
    [SerializeField] private CycleState cycleState;
    public CycleState GetCycleState()
    {
        return cycleState;
    }
    private float countdown = 240f; //four mins
    [SerializeField]private TextMeshProUGUI cycleStateText;
    [SerializeField]private float[] setKindomCycleCount = new float[5];
    [SerializeField]private float cycleDecrement;
    [SerializeField]private int cycleCount;
    private Dictionary<CycleState, float> groupStatesWithFloat = new Dictionary<CycleState, float>();

    // Start is called before the first frame update
    void Start()
    {
        cycleState = CycleState.DawnToSunrise;
        cycleStateText.text = cycleState.ToString();
        OnStateChanged += OnStateChanged_ChangeStateText;
        OnDawnStart += OnStateChanged_DawnStarted;
        OnNightStart += OnStateChanged_NightStarted;
        groupStatesWithFloat.Add(CycleState.DawnToSunrise, setKindomCycleCount[0]);
        groupStatesWithFloat.Add(CycleState.SunriseToNoon, setKindomCycleCount[1]);
        groupStatesWithFloat.Add(CycleState.NoonToSunset, setKindomCycleCount[2]);
        groupStatesWithFloat.Add(CycleState.SunsetToMidnight, setKindomCycleCount[3]);
        groupStatesWithFloat.Add(CycleState.MidnightToDawn, setKindomCycleCount[4]);
        cycleCount = 0;
        cycleDecrement = groupStatesWithFloat[CycleState.DawnToSunrise];

    }

    // Update is called once per frame
    void Update()
    {
        DifferentWayToDoCycleState();
    }
    private void DifferentWayToDoCycleState()
    {
        cycleDecrement -= Time.deltaTime;
        if (cycleDecrement <= 0)
        {
            cycleCount++;
            if (cycleCount > 4)
            {
                cycleCount = 0;
            }
            OnStateChanged?.Invoke(this, EventArgs.Empty);
            cycleDecrement = setKindomCycleCount[cycleCount];
        }
    }
    private void OnStateChanged_DawnStarted(object sender, EventArgs e)
    {
        Debug.Log("Dawn has Started");
    }
    private void OnStateChanged_NightStarted(object sender, EventArgs e)
    {
        Debug.Log("Night has Started");
    }
    private void OnStateChanged_ChangeStateText(object sender, EventArgs e)
    {
        Debug.Log("Cycle State has Changed");
        if(cycleState == CycleState.DawnToSunrise)
        {
            cycleState = CycleState.SunriseToNoon;
        }
        else if (cycleState == CycleState.SunriseToNoon)
        {
            cycleState = CycleState.NoonToSunset;
        }
        else if(cycleState == CycleState.NoonToSunset)
        {
            cycleState = CycleState.SunsetToMidnight;
        }
        else if(cycleState == CycleState.SunsetToMidnight)
        {
            cycleState = CycleState.MidnightToDawn;
        }
        else if(cycleState == CycleState.MidnightToDawn)
        {
            cycleState = CycleState.DawnToSunrise;
        }
        cycleStateText.text = cycleState.ToString();
        if(cycleCount == 0)
        {
            OnDawnStart?.Invoke(this, EventArgs.Empty); 
        }
        if(cycleCount == 3)
        {
            OnNightStart?.Invoke(this, EventArgs.Empty);
        }
    }
}
