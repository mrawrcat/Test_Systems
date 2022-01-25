using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlTest : MonoBehaviour
{

    [SerializeField]
    private LevelWindow lvlWindow;
    private void Awake()
    {
        LevelSystem levelSystem = new LevelSystem();
        lvlWindow.SetLevelSystem(levelSystem);
        LevelSystemAnimated levelSystemAnimated = new LevelSystemAnimated(levelSystem);
        lvlWindow.SetLevelSystemAnimated(levelSystemAnimated);
    }
}
