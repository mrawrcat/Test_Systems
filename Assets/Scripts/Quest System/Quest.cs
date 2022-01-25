using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Quest : MonoBehaviour
{

    public List<Goal> Goals = new List<Goal>();
    public string Quest_Description;
    public int Exp_Reward;
    public bool Quest_Completed;

    public void Check_Goals()
    {
        Quest_Completed = Goals.All(g => g.Completed);
    }

    public void Give_Reward()
    {
        Debug.Log("completed quest give reward");
    }
}
