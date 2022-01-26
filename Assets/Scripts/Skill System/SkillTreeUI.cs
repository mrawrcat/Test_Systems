using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class SkillTreeUI : MonoBehaviour
{
    
    private PlayerSkills playerSkills;
    [SerializeField]
    private Text skillPointText;
    [SerializeField]
    private GameObject[] btns;
    private void Awake()
    {
       
        btns[0].GetComponent<Button_UI>().ClickFunc = () => { playerSkills.TryUnlockSkill(PlayerSkills.SkillType.increasedBulletCount);  gameObject.SetActive(false); };
        btns[1].GetComponent<Button_UI>().ClickFunc = () => { playerSkills.TryUnlockSkill(PlayerSkills.SkillType.airShot);  gameObject.SetActive(false); };
    }

    public void SetPlayerSkills(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
        playerSkills.OnSkillPointsChanged += PlayerSkills_OnSkillPointChanged;
        UpdateSkillPointVisual();
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
    {
        Debug.Log("skill unlocked");
    }

    private void PlayerSkills_OnSkillPointChanged(object sender, System.EventArgs e)
    {
        UpdateSkillPointVisual();
    }

    private void UpdateSkillPointVisual()
    {
        skillPointText.text = "Skill Points: " + playerSkills.GetSkillPoints().ToString();
    }
}
