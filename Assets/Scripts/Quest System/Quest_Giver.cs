using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Quest_Giver : MonoBehaviour
{
    public bool Assigned_Quest;
    public bool Helped;
    public int how_many_quests;
    [SerializeField]
    private string[] quest_script_name;
    public GameObject quest;
    private Quest Quest;
    public string quest_name_desc;
    public Text descTxt;
    public Text isQuestComplete;
    public RectTransform descTransform;
    public bool showedQuest = false;
    public int questNum;
    [Header("prevent dirty")]
    public GameObject prevent_dirty_canvas;


    public float timeOfTravel = 5; //time after object reach a target place 
    public float currentTime = 0; // actual floting time 
    float normalizedValue;
    public Vector2 rectTrans_hide;
    public Vector2 rectTrans_show;

    void Start()
    {
        questNum = Random.Range(0, how_many_quests);
        //Talk_to_Quest_Giver();
        //descTransform.anchoredPosition = new Vector2(0, 300);


    }

    void Update()
    {
        Debug.Log("always updating" + quest.GetComponent<Quest>().Quest_Description);
        quest_name_desc = Quest.Quest_Description;
        descTxt.text = quest_name_desc;

        
        if (!showedQuest)
        {
            prevent_dirty_canvas.SetActive(true);

        }
        else
        {
            prevent_dirty_canvas.SetActive(false);
        }
       
    }

    public void Talk_to_Quest_Giver()
    {
        if(!Assigned_Quest && !Helped)
        {
            Debug.Log("Got Quest");
            Assign_Quest();
            showedQuest = false;
            StartCoroutine(Show_And_Hide_Quest(rectTrans_hide, rectTrans_show));

        }
        else if(Assigned_Quest && !Helped)
        {
            Debug.Log("already have quest");
            Giver_Checks_Quest();
        }
        
    }

    void Assign_Quest()
    {
        Assigned_Quest = true;
        Quest = (Quest)quest.AddComponent(System.Type.GetType(quest_script_name[questNum]));
        isQuestComplete.text = "Quest Incomplete";
    }

    void Giver_Checks_Quest()
    {
        if (Quest.Quest_Completed)
        {
            isQuestComplete.text = "Quest Complete";
            Quest.Give_Reward();
            Helped = true;
            Assigned_Quest = false;
        }
    }

    public void Forfeit_Quest()
    {
        Assigned_Quest = false;
        Helped = false;

    }
    IEnumerator Show_Quest()
    {
        //descTransform.anchoredPosition = new Vector2(0, 300);
        prevent_dirty_canvas.SetActive(true);
        yield return new WaitForSeconds(1f);
        descTransform.anchoredPosition = Vector2.Lerp(descTransform.anchoredPosition, new Vector2(0, 400), 5f * Time.deltaTime);
        yield return new WaitForSeconds(1f);
        showedQuest = true;

    }

    IEnumerator Hide_Quest()
    {
        yield return new WaitForSeconds(1f);
        descTransform.anchoredPosition = Vector2.Lerp(descTransform.anchoredPosition, new Vector2(0, 700), 5f * Time.deltaTime);
        yield return new WaitForSeconds(1f);
        prevent_dirty_canvas.SetActive(false);
    }

    public void Move_Quest_Text()
    {
        descTransform.anchoredPosition = Vector2.Lerp(descTransform.anchoredPosition, new Vector2(0, 0), 5f * Time.deltaTime);
    }

    IEnumerator Show_And_Hide_Quest(Vector2 start_pos, Vector2 end_pos)
    {
        currentTime = 0;
        yield return new WaitForSeconds(1f);
        while (currentTime <= timeOfTravel)
        {
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / timeOfTravel; // we normalize our time 
            descTransform.anchoredPosition = Vector3.Lerp(start_pos, end_pos, normalizedValue);
            yield return null;
        }
        if (descTransform.anchoredPosition == end_pos)
        {
            currentTime = 0;
            yield return new WaitForSeconds(1f);
            while (currentTime <= timeOfTravel)
            {
                currentTime += Time.deltaTime;
                normalizedValue = currentTime / timeOfTravel; // we normalize our time 
                descTransform.anchoredPosition = Vector3.Lerp(end_pos, start_pos, normalizedValue);
                yield return null;
            }
            if (descTransform.anchoredPosition == start_pos)
            {
                showedQuest = true;
            }
        }
    }

    IEnumerator LerpObject(Vector2 start_pos, Vector2 end_pos)
    {
        showedQuest = false;
        //yield return new WaitForSeconds(1f);
        while (currentTime <= timeOfTravel)
        {
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / timeOfTravel; // we normalize our time 
            descTransform.anchoredPosition = Vector3.Lerp(start_pos, end_pos, normalizedValue);
            yield return null;
        }
        if(descTransform.anchoredPosition == end_pos)
        {
            showedQuest = true;
        }

    }

    
}
