using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceObject : MonoBehaviour
{
    private static List<ResourceObject> resourceObjectList;
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    public static ResourceObject GetClosestResourceObject(Vector3 position, float maxRange)
    {
        ResourceObject closest = null;
        foreach (ResourceObject resourceObj in resourceObjectList)
        {
            if (Vector3.Distance(position, resourceObj.GetPosition()) <= maxRange)
            {
                if(closest = null)
                {
                    closest = resourceObj;
                }
                else
                {
                    float currentClosest = Vector3.Distance(position, resourceObj.GetPosition());
                    float currentChecking = Vector3.Distance(position, closest.GetPosition());
                    if (currentChecking < currentClosest)
                    {
                        closest = resourceObj;
                    }
                }
            }
        }
        return closest;
    }
    
    private void Awake()
    {
        if(resourceObjectList == null)
        {
            resourceObjectList = new List<ResourceObject>();
        }
        resourceObjectList.Add(this);
    }
}
