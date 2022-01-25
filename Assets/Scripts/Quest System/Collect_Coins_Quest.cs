using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect_Coins_Quest : Quest
{
    void Start()
    {
        Quest_Description = "Collect 50 Coins";

        Goals.Add(new Collect_Goal(this, "Coin", "Collect 50 Coins", false, 0, 50));

        Goals.ForEach(g => g.Init());
    }
}
