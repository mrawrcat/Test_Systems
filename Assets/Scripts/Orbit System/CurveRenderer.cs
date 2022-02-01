using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveRenderer : MonoBehaviour
{
    //[SerializeField] private Transform[] point;
    private List<Vector3> pointList = new List<Vector3>();
    [SerializeField] private float lineWidth = .1f;
    private LineRenderer lineRenderer;
    private int vertexCount = 12;
    //private Transform thisTransform;

    // Use this for initialization
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            pointList.Add(transform.GetChild(i).transform.position);
        }
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    // Update is called once per frame
    void Update()
    {
        var pointListCalculate = new List<Vector3>();
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(pointList[0], pointList[1], ratio);
            var tangentLineVertex2 = Vector3.Lerp(pointList[1], pointList[2], ratio);
            var bezierpoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointListCalculate.Add(bezierpoint);
        }
        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(pointList[0], pointList[1]);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(pointList[1], pointList[2]);

        Gizmos.color = Color.red;
        for (float ratio = 0.5f / vertexCount; ratio < 1; ratio += 1.0f / vertexCount)
        {
            Gizmos.DrawLine(Vector3.Lerp(pointList[0], pointList[1], ratio), Vector3.Lerp(pointList[1], pointList[2], ratio));
        }
    }
    */
}
