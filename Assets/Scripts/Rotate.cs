using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    private float speed;

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(new Vector3(0, 0, speed) * Time.deltaTime);
        transform.RotateAround(transform.parent.transform.position, Vector3.forward, 20 * Time.deltaTime);
    }
}
