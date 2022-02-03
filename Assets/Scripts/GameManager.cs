using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    public float coins;
    public float total_coins;



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

       
    }

    

    public void SaveData()
    {
        SaveDataSystem.SaveGameData(this);
    }
    public void LoadData()
    {
        StoreData data = SaveDataSystem.loadData();

        total_coins = data.total_coins;
    }
}
