using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect_Goal : Goal
{

    public string itemID { get; set; }

    public Collect_Goal(Quest quest, string itemID, string description, bool completed, int current_amt, int required_amt)
    {
        this.Quest = quest;
        this.itemID = itemID;
        this.Description = description;
        this.Completed = completed;
        this.Current_Amount = current_amt;
        this.Required_Amount = required_amt;
    }

    public override void Init()
    {
        base.Init();
        CollectionEvents.OnCollectableCollected += Collectable_Collected;
    }

    void Collectable_Collected(ICollectable collectable)
    {
        if(collectable.itemID == this.itemID)
        {
            this.Current_Amount++;
            Evaluate();
        }
    }
}
