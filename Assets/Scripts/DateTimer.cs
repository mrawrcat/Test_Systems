using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DateTimer : MonoBehaviour
{
    DateTime currentDate;
    DateTime oldDate;
    public TextMeshProUGUI timetext, cabbagetext;
    public float countdown;

    public int cabbages;
    public bool readytoharvest;
    private float savedtime;
    void Start()
    {
        countSavedTime();
    }
    private void Update()
    {
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
        }
        else if (countdown <= 0)
        {
            readytoharvest = true;
        }
        Showtext();
    }
    private void OnMouseDown()
    {
        Harvest();
    }
    public void Harvest()
    {
        if (readytoharvest == true)
        {
            cabbages += 50;
            countdown += 60f;
            readytoharvest = false;
        }
    }
    void OnApplicationQuit()
    {
        quitSave();
        Debug.Log("saved");
    }
    public void quitSave()
    {
        //Save the current system time as a string in the player prefs class
        PlayerPrefs.SetString("sysString", System.DateTime.Now.ToBinary().ToString());
        PlayerPrefs.SetFloat("TimeOnExit", countdown);
        PlayerPrefs.SetInt("hasCabbages", cabbages);

        Debug.Log("Saving this date to prefs: " + System.DateTime.Now);
        Debug.Log("has this amt of cabbages: " + cabbages);
    }
    void Showtext()
    {
        timetext.text = "Time Remaining: " + Mathf.Round(countdown);
        cabbagetext.text = "Sunflower Seeds: " + cabbages;
    }
    public void spendSeeds()
    {
        if (cabbages >= 100)
        {
            cabbages -= 100;
        }

    }
    public void countSavedTime() //at start of game, find out how much time has passed and subtract to see if countdown is at 0
    {
        if (PlayerPrefs.HasKey("hasCabbages"))
        {
            cabbages = PlayerPrefs.GetInt("hasCabbages");
        }

        //Store the current time when it starts
        currentDate = System.DateTime.Now;

        //Grab the old time from the player prefs as a long
        long temp = Convert.ToInt64(PlayerPrefs.GetString("sysString"));

        //Convert the old time from binary to a DataTime variable
        DateTime oldDate = DateTime.FromBinary(temp);

        Debug.Log("oldDate: " + oldDate);

        //Use the Subtract method and store the result as a timespan variable
        TimeSpan difference = currentDate.Subtract(oldDate);

        Debug.Log("Difference: " + difference);

        float floatTimeSpan;
        int seconds, milliseconds, minutes;
        minutes = difference.Minutes;
        seconds = difference.Seconds;
        milliseconds = difference.Milliseconds;
        floatTimeSpan = ((float)minutes * 60) + (float)seconds + ((float)milliseconds / 1000);

        Debug.Log("converted float: " + floatTimeSpan);
        Debug.Log("total seconds: " + difference.TotalSeconds);

        if (PlayerPrefs.HasKey("TimeOnExit"))
        {
            savedtime = PlayerPrefs.GetFloat("TimeOnExit");
            if ((savedtime - floatTimeSpan) < 0)
            {
                countdown = 0;
            }
            else
            {
                countdown = savedtime -= floatTimeSpan;
            }

            PlayerPrefs.DeleteKey("TimeOnExit");
        }
    }
}
