using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Goal
{
    public Quest Quest { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }
    public int Current_Amount { get; set; }
    public int Required_Amount { get; set; }

    public virtual void Init()
    {
        //default init stuff
    }
    public void Evaluate()
    {
        if (Current_Amount >= Required_Amount)
        {
            Debug.Log("quest current amt obtained" + Current_Amount);
            Complete();
        }
    }

    public void Complete()
    {
        Completed = true;
        Quest.Check_Goals();
    }

}
