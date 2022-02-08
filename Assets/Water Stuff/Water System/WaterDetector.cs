using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D Hit)
    {
        Rigidbody2D rb2d = Hit.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            //Water_Using_Springs waterObj = rb2d.gameObject.GetComponent<Water_Using_Springs>();
            transform.parent.GetComponent<Water_Using_Springs>().Splash(transform.position.x, rb2d.velocity.y * rb2d.mass / 40f);
        }
    }
}
