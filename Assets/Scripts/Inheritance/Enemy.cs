using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private string[] tags;
    [SerializeField]
    private float health = 1;
    [SerializeField]
    private float scoreGain;
    private DropPool dropPool;
    private CoinPool coinPool;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        for (int i = 0; i < tags.Length; i++)
        {
            if (collision.collider.tag == tags[i])
            {
                //EnemyActions.OnEnemyHit(this);
                if(health <= 0)
                {
                    //EnemyActions.OnEnemyDie(this);
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
                    Disappear();
                }

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        for (int i = 0; i < tags.Length; i++)
        {
            if (collision.tag == tags[i])
            {
                //EnemyActions.OnEnemyHit(this);
                if (health <= 0)
                {
                    
                    //EnemyActions.OnEnemyDie(this);
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
                    Disappear();
                }

            }
        }
    }

    public void TakeDmg(float dmg)
    {
        health -= dmg;
    }

    public void SetHealth(float sethealth)
    {
        health = sethealth;
    }

    public void Disappear()
    {
        gameObject.SetActive(false);
    }
}
