using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defeat_Boss_Quest : Quest
{
    void Start()
    {
        Quest_Description = "Defeat A Boss";

        Goals.Add(new Kill_Goal(this, 2, "Defeat A Boss", false, 0, 1));

        Goals.ForEach(g => g.Init());
    }
}
