using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoreData
{

    [Header("Stats")]
    public float total_coins;

    public StoreData(GameManager manager)
    {
        total_coins = manager.total_coins;

    }
}