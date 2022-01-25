using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem
{
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;

    private int level;
    private int experience;
    private int expToNextLvl;

    public LevelSystem()
    {
        level = 0;
        experience = 0;
        expToNextLvl = 100;
    }

    public void Add_Experience(int amt)
    {
        experience += amt;
        while(experience >= expToNextLvl)
        {
            level++;
            experience -= expToNextLvl;
            if(OnLevelChanged != null)
            {
                OnLevelChanged(this, EventArgs.Empty);
            }
        }
        if(OnExperienceChanged != null)
        {
            OnExperienceChanged(this, EventArgs.Empty);
        }
    }

    public int GetLevelNumber()
    {
        return level;
    }

    public float GetExpNormalized()
    {
        return (float)experience / expToNextLvl;
    }

    public int GetExpToNextLvl()
    {
        return expToNextLvl;
    }

    public int GetExperience()
    {
        return experience;
    }
}
