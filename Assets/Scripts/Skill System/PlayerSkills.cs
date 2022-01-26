    using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills
{
    public event EventHandler OnSkillPointsChanged;
    public event EventHandler<OnSkillUnlockedEventArgs> OnSkillUnlocked;
    public class OnSkillUnlockedEventArgs : EventArgs
    {
        public SkillType skillType;
    }
    public enum SkillType
    {
        None,
        increasedBulletCount,
        doubleJump,
        airShot,
    }

    private List<SkillType> unlockedSkillTypeList;

    private int skillPoints;
    public PlayerSkills()
    {
        unlockedSkillTypeList = new List<SkillType>();
    }

    private void UnlockSkill(SkillType skillType)
    {
        if (!IsSkillUnlocked(skillType))
        {
            unlockedSkillTypeList.Add(skillType);
            OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs { skillType = skillType});
        }
    }

    public void Add_SkillPoint()
    {
        skillPoints++;
        OnSkillPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsSkillUnlocked(SkillType skillType)
    {
        return unlockedSkillTypeList.Contains(skillType);
    }

    public SkillType GetSkillRequirement(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.doubleJump:
                return SkillType.airShot;
        }
        return SkillType.None;
    }
    
    public bool TryUnlockSkill(SkillType skillType)
    {
        if (CanUnlockSkill(skillType))
        {
            if(skillPoints > 0)
            {
                if (IsSkillUnlocked(skillType) == false)
                {
                    skillPoints--;
                    OnSkillPointsChanged?.Invoke(this, EventArgs.Empty);
                    UnlockSkill(skillType);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public bool CanUnlockSkill(SkillType skillType)
    {
        SkillType skillRequirement = GetSkillRequirement(skillType);

        if (skillRequirement != SkillType.None)
        {
            if (IsSkillUnlocked(skillRequirement))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    public int GetSkillPoints()
    {
        return skillPoints;
    }
}
