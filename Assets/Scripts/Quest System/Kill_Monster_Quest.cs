using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill_Monster_Quest : Quest
{
    // Start is called before the first frame update
    void Start()
    {
        Quest_Description = "Kill 5 Enemies";

        Goals.Add(new Kill_Goal(this, 0, "Kill 5 Enemies", false, 0, 5));

        Goals.ForEach(g => g.Init());
    }

   
}
