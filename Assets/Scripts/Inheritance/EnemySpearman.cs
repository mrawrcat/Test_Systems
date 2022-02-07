using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpearman : Characters
{
    [SerializeField]
    private Transform walkToTargetTransform;
    // Start is called before the first frame update
    void Start()
    {
        SetHealth(100);
        walkToTargetTransform = FindObjectOfType<Town_Center>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //this is logic for enemy hitting workers/structures; logic for arrows hitting enemy should be in arrow class
        IDamagableByEnemy damagableObj = collision.gameObject.GetComponent<IDamagableByEnemy>();
        if(damagableObj != null)
        {
            damagableObj.Damage();
        }
    }
}
