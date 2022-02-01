using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dome : MonoBehaviour
{
    public int segments;
    public float xradius;
    public float yradius;
    public float roundAngle = 360f;
    private LineRenderer line;

    private void Start()
    {
        
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = (segments + 1);
        line.useWorldSpace = false;
        Fire();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Fire();
        }
    }

    //function for firing the laser
    public void Fire()
    {
        line.positionCount = (segments + 1);
        float x = 0f;
        float y = 0f;
        float z;
        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            line.SetPosition(i, new Vector3(x, y, z));

            angle += (roundAngle / segments);
        }
    }
}
