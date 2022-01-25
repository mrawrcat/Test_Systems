using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Boxes_Quest : Quest
{
    void Start()
    {
        Quest_Description = "Destroy 5 Boxes";

        Goals.Add(new Kill_Goal(this, 1, "Destroy 5 Boxes", false, 0, 5));

        Goals.ForEach(g => g.Init());
    }
}
