using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class LevelWindow : MonoBehaviour
{
    [SerializeField]
    private Text lvlTxt;
    [SerializeField]
    private Image expBarFill;

    [SerializeField]
    private Image[] buttons;

    private LevelSystem levelSystem;
    private LevelSystemAnimated levelSystemAnimated;
    private void Awake()
    {
        //lvlTxt = transform.Find("lvl Txt").GetComponent<Text>();
        //expBarFill = transform.Find("lvl bar fill").GetComponent<Image>();

        buttons[0].GetComponent<Button_UI>().ClickFunc = () => levelSystem.Add_Experience(5);
        buttons[1].GetComponent<Button_UI>().ClickFunc = () => levelSystem.Add_Experience(50);
        buttons[2].GetComponent<Button_UI>().ClickFunc = () => levelSystem.Add_Experience(500);
    }

    

    private void SetLevelNum(int lvlNum)
    {
        lvlTxt.text = "Level\n" + (lvlNum + 1);
    }

    private void SetExpBar(float expNormalized)
    {
        expBarFill.fillAmount = expNormalized;
    }

    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;
    }

    public void SetLevelSystemAnimated(LevelSystemAnimated levelSystemAnimated)
    {
        this.levelSystemAnimated = levelSystemAnimated;

        //update starting values
        SetLevelNum(levelSystemAnimated.GetLevelNumber());
        SetExpBar(levelSystemAnimated.GetExpNormalized());

        //listens to when exp and lvl changed
        levelSystemAnimated.OnLevelChanged += LevelSystemAnimated_OnLevelChanged;
        levelSystemAnimated.OnExperienceChanged += LevelSystemAnimated_OnExperienceChanged;
    }
    private void LevelSystemAnimated_OnLevelChanged(object sender, System.EventArgs e)
    {
        SetLevelNum(levelSystemAnimated.GetLevelNumber());
    }
    private void LevelSystemAnimated_OnExperienceChanged(object sender, System.EventArgs e)
    {
        SetExpBar(levelSystemAnimated.GetExpNormalized());
    }

}
