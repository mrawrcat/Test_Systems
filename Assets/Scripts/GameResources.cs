using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources _instance;
    public static GameResources instance
    {
        get 
        {
            if(_instance == null)
            {
                _instance = (Instantiate(Resources.Load("GameResources")) as GameObject).GetComponent<GameResources>();
            }
            return _instance;
        }
    }

    public Transform arrowPrefab;
    public Transform arrow;
    public Transform Bandit;
}
