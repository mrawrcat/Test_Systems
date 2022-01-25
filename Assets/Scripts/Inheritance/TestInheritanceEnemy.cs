using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInheritanceEnemy : Characters
{
   
  
    public override void Killed()
    {
        base.Killed();
        Debug.Log("i am inheritanceEnemy");
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < tags.Length; i++)
        {
            if (collision.collider.tag == tags[i])
            {
                EnemyActions.OnIEnemyHit(this);
                if (health <= 0)
                {
                    
                    EnemyActions.OnIEnemyDie(this);
                    float rand = Random.Range(0, 4);
                    if (rand < 2)
                    {
                        dropPool = FindObjectOfType<DropPool>();
                        dropPool.SpawnDrop(transform);
                    }
                    else
                    {
                        coinPool = FindObjectOfType<CoinPool>();
                        coinPool.SpawnCoin(transform);
                    }
                    Killed();
                }

            }
        }
    }
}
