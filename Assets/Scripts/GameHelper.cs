using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GameHelper : MonoBehaviour
{
    
    [Header("Text stuff")]
    public Text coins;
    

    
    

    private float distLerpHolder;
    private float coinsLerpHolder;
    private float lerpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.manager.LoadData();
       
    }

    // Update is called once per frame
    void Update()
    {
        show_text();        
    }
    
    public void LoadGame()
    {
        GameManager.manager.LoadData();
    }
    public void ClearData()
    {
        GameManager.manager.coins = 0;
    }

    public void Add_Coins()
    {
        GameManager.manager.coins += 100;
    }
    public void ToggleDist()
    {
        GameManager.manager.can_move_distance = !GameManager.manager.can_move_distance;
    }
    public void New_Game()
    {
        GameManager.manager.coins = 0;
    }

    public void SaveData()
    {
        GameManager.manager.total_coins += GameManager.manager.coins;
        GameManager.manager.SaveData();
    }
    private void show_text()
    {
        coins.text = "Coins: " + GameManager.manager.coins.ToString("F0");
    }

    public void Move_Continue()
    {
        //reviveButtonRect.anchoredPosition = Vector2.Lerp(reviveButtonRect.anchoredPosition, new Vector2(0, 0), 5f * Time.deltaTime);
    }

    public void MoveDeadStats()
    {
        //deadStats.anchoredPosition = Vector2.Lerp(deadStats.anchoredPosition, new Vector2(0, 65), 5f * Time.deltaTime);
        //distLerpHolder = Mathf.Lerp(0, GameManager.manager.distanceMoved, lerpSpeed);
        //coinsLerpHolder = Mathf.Lerp(0, GameManager.manager.coins, lerpSpeed);
    }

    

    
}
