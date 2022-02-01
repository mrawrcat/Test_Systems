using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuilding : MonoBehaviour
{
    [SerializeField] private Sprite upgrade0;
    [SerializeField] private Sprite upgrade1;
    [SerializeField] private Sprite upgrade2;
    [SerializeField] private Sprite upgrade0H;
    [SerializeField] private Sprite upgrade1H;
    [SerializeField] private Sprite upgrade2H;

    private enum State
    {
        upgrade0,
        upgrade1,
        upgrade2,
    }

    private State state;
    [SerializeField] private bool stay;
    private int upgradeState = 0;
    private int maxUpgradeState = 2;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        state = State.upgrade0;
        upgradeState = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = upgrade0;
    }

    // Update is called once per frame
    void Update()
    {
        if (stay)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                if(upgradeState < maxUpgradeState)
                {
                    upgradeState++;
                }
            }
        }
        IsStayHighlight();
    }

    private void IsStayHighlight()
    {
        if (stay)
        {
            if(upgradeState == 0)
            {
                spriteRenderer.sprite = upgrade0H;
            }
            else if (upgradeState == 1)
            {
                spriteRenderer.sprite = upgrade1H;
            }
            else if (upgradeState == 2)
            {
                spriteRenderer.sprite = upgrade2H;
            }
        }
        else
        {
            if (upgradeState == 0)
            {
                spriteRenderer.sprite = upgrade0;
            }
            else if (upgradeState == 1)
            {
                spriteRenderer.sprite = upgrade1;
            }
            else if (upgradeState == 2)
            {
                spriteRenderer.sprite = upgrade2;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            stay = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            stay = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            stay = false;
        }
    }

}
