using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionEvents : MonoBehaviour
{
    public delegate void CollectEventHandler(ICollectable collectable);
    public static event CollectEventHandler OnCollectableCollected;

    public static void CollectableCollected(ICollectable collectable)
    {
        if (OnCollectableCollected != null)
        {
            OnCollectableCollected(collectable);
        }
    }
}
