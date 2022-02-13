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
        GameObtainableResources.Init();//put this in game handler later
        GameObtainableResources.OnResourceAmountChanged += OnResourceAmountChanged;
        UpdateResourceText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameObtainableResources.AddResourceAmount(GameObtainableResources.ResourceType.Gold, 1);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameObtainableResources.AddResourceAmount(GameObtainableResources.ResourceType.Gold, 10);
        }
    }

    private void UpdateResourceText()
    {
        goldAmtTxt.text = "Gold: " + GameObtainableResources.GetResourceAmount(GameObtainableResources.ResourceType.Gold);
    }

    private void OnResourceAmountChanged(object sender, System.EventArgs e)
    {
        UpdateResourceText();
        Debug.Log("OnResourceAmountChanged");
    }
}
