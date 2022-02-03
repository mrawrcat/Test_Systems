using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameResources : MonoBehaviour
{
    [SerializeField] private Text goldAmtTxt;

    private void Awake()
    {
        GameResources.Init();//put this in game handler later
        GameResources.OnResourceAmountChanged += OnResourceAmountChanged;
        UpdateResourceText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameResources.AddResourceAmount(GameResources.ResourceType.Gold, 1);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameResources.AddResourceAmount(GameResources.ResourceType.Gold, 10);
        }
    }

    private void UpdateResourceText()
    {
        goldAmtTxt.text = "Gold: " + GameResources.GetResourceAmount(GameResources.ResourceType.Gold);
    }

    private void OnResourceAmountChanged(object sender, System.EventArgs e)
    {
        UpdateResourceText();
        Debug.Log("OnResourceAmountChanged");
    }
}
