using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testheaderorder : MonoBehaviour
{
    [Space(10, order = 0)]
    [Header("Header with some space around it", order = 1)]
    public int blue = 2;
    [Space(40, order = 2)]

    public string playerName = "Unnamed";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
