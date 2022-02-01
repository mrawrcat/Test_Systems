using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveRendererTransform : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public Transform point3;
    public int vertexCount = 12;
    [SerializeField] private float lineWidth = .1f;
    private Vector2[] points = new Vector2[3];
    private LineRenderer lineRenderer;


    // Use this for initialization
    void Start()
    {
        points[0] = point1.position;
        points[1] = point2.position;
        points[2] = point3.position;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        gameObject.AddComponent<PolygonCollider2D>();
        gameObject.GetComponent<PolygonCollider2D>().isTrigger = true;
        gameObject.GetComponent<PolygonCollider2D>().SetPath(0, points);

    }

    // Update is called once per frame
    void Update()
    {
        var pointList = new List<Vector3>();
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            var tangentLineVertex1 = Vector3.Lerp(point1.position, point2.position, ratio);
            var tangentLineVertex2 = Vector3.Lerp(point2.position, point3.position, ratio);
            var bezierpoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bezierpoint);
        }
        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(point1.position, point2.position);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(point2.position, point3.position);

        Gizmos.color = Color.red;
        for (float ratio = 0.5f / vertexCount; ratio < 1; ratio += 1.0f / vertexCount)
        {
            Gizmos.DrawLine(Vector3.Lerp(point1.position, point2.position, ratio), Vector3.Lerp(point2.position, point3.position, ratio));
        }
    }
}
