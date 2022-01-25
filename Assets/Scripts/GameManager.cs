using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public float coins;
    public float distance;
    public float total_coins;
    public float best_distance;



    public bool can_move_distance;
    
    
    
    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (can_move_distance)
        {
            distance+= Time.deltaTime;
        }

        if(distance > best_distance)
        {
            best_distance = distance;
        }
    }

    

    public void SaveData()
    {
        SaveDataSystem.SaveGameData(this);
    }
    public void LoadData()
    {
        StoreData data = SaveDataSystem.loadData();

        total_coins = data.total_coins;
        best_distance = data.best_distance;
    }
}
