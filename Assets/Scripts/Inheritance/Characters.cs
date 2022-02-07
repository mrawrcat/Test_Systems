using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Characters : MonoBehaviour, IDamagable<int>, IKillable
{
    public int health;
    public float scoreGain;
    public string[] tags;
    public DropPool dropPool;
    public CoinPool coinPool;

    private void Start()
    {
        //dropPool = FindObjectOfType<DropPool>();
        //coinPool = FindObjectOfType<CoinPool>();
    }

    public void SetHealth(int sethealth)
    {
        health = sethealth;
    }

    public virtual void Killed()
    {
        Debug.Log("i am character");
        //gameObject.SetActive(false);

    }
    public void SetPools()
    {
        dropPool = FindObjectOfType<DropPool>();
        coinPool = FindObjectOfType<CoinPool>();
    }
    public void TakeDmg(int dmgTaken)
    {
        health -= dmgTaken;
    }
}
