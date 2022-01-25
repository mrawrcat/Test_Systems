using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    public string itemID { get; set; }
    public Transform start_pos;

    // Start is called before the first frame update
    void Start()
    {
        itemID = "Coin";
    }

    public void Collect()
    {
        GameManager.manager.coins++;
        CollectionEvents.CollectableCollected(this);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("coin touched player");
            Collect();
        }
    }
}
