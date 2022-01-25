using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEnemy : MonoBehaviour, IEnemy
{
    public int ID { get; set; }

    private float health;
    //private PlayerDeath player_death;
    // Start is called before the first frame update
    void Start()
    {
        ID = 0;
        //player_death = FindObjectOfType<PlayerDeath>();
    }

    /*
    public void Take_Dmg(float dmg_amt)
    {
        health -= dmg_amt;
        if(health <= 0)
        {
            CombatEvents.EnemyDied(this);
            gameObject.SetActive(false);
        }
    }
    */
    public void Die()
    {
        CombatEvents.EnemyDied(this);
        Debug.Log("killed generic enemy");
        gameObject.SetActive(false);

    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            Debug.Log("collided with player");
            if(GameManager.manager.invincible <= 0)
            {
                player_death.Die();
            }
            else
            {
                Die();
            }
        }
        if(collision.collider.tag == "PlayerProjectile")
        {
            Die();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("touching player");
            if (GameManager.manager.invincible <= 0)
            {
                player_death.Die();
            }
            else
            {
                Die();
            }
        }
        if (collision.collider.tag == "PlayerProjectile")
        {
            Die();
        }
    }
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            
        }
        if (collision.tag == "PlayerProjectile")
        {
            Die();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            
            

        }
        if (collision.tag == "PlayerProjectile")
        {
            Die();
        }
    }
}
