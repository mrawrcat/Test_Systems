using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class LevelSystemAnimated
{
    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;

    private LevelSystem levelSystem;
    private bool isAnimating;

    private float updateTimer;
    private float updateTimerMax;

    private int level;
    private int experience;
    private int expToNextLvl;

    public LevelSystemAnimated(LevelSystem levelSystem)
    {
        SetLevelSystem(levelSystem);
        updateTimerMax = .016f;

        FunctionUpdater.Create(() => Update());
    }

    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;

        level = levelSystem.GetLevelNumber();
        experience = levelSystem.GetExperience();
        expToNextLvl = levelSystem.GetExpToNextLvl();

        levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, System.EventArgs e)
    {
        isAnimating = true;
    }

    private void LevelSystem_OnExperienceChanged(object sender, System.EventArgs e)
    {
        isAnimating = true;
    }

    private void Update()
    {
        if (isAnimating)
        {
            updateTimer += Time.deltaTime;
            while(updateTimer > updateTimerMax)
            {
                updateTimer -= updateTimerMax;
                UpdateAddExperience();
            }
            
        }
    }

    private void UpdateAddExperience()
    {
        if (level < levelSystem.GetLevelNumber())
        {
            Add_Experience();
        }
        else
        {
            if (experience < levelSystem.GetExperience())
            {
                Add_Experience();
            }
            else
            {
                isAnimating = false;
            }
        }
    }

    private void Add_Experience()
    {
        experience++;
        if(experience >= expToNextLvl)
        {
            level++;
            experience = 0;
            if (OnLevelChanged != null)
            {
                OnLevelChanged(this, EventArgs.Empty);
            }
        }
        if (OnExperienceChanged != null)
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
