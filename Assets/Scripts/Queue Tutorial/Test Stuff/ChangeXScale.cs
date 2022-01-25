using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeXScale : MonoBehaviour
{
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = transform.parent.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = playerTransform.localScale;
        //Debug.Log(transform.localScale);
    }
}
