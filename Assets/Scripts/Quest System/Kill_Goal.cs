using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill_Goal : Goal
{

    public int EnemyID { get; set; }

    public Kill_Goal(Quest quest, int enemyID, string description, bool completed, int current_amt, int required_amt)
    {
        this.Quest = quest;
        this.EnemyID = enemyID;
        this.Description = description;
        this.Completed = completed;
        this.Current_Amount = current_amt;
        this.Required_Amount = required_amt;
    }

    public override void Init()
    {
        base.Init();
        CombatEvents.OnEnemyDeath += Enemy_Died;
    }

    void Enemy_Died(IEnemy enemy)
    {
        if(enemy.ID == this.EnemyID)
        {
            this.Current_Amount++;
            Evaluate();
        }
    }
}
