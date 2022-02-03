using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallBuilding : MonoBehaviour
{
    [SerializeField] private Sprite upgrade0;
    [SerializeField] private Sprite upgrade1;
    [SerializeField] private Sprite upgrade2;
    [SerializeField] private Sprite upgrade0H;
    [SerializeField] private Sprite upgrade1H;
    [SerializeField] private Sprite upgrade2H;

    [SerializeField]private RectTransform childUI;
    //private float buildingHealth = 100f;
   


    [SerializeField] private bool stay;
    private int upgradeState = 0;
    private int maxUpgradeState = 2;
    private SpriteRenderer spriteRenderer;
    private int currentUpgradeRequirements;
    private bool canUpgrade()
    {
        if(GameResources.GetResourceAmount(GameResources.ResourceType.Gold) > currentUpgradeRequirements)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        upgradeState = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = upgrade0;
        currentUpgradeRequirements = 1;
        //childUI = gameObject.GetComponentInChildren<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stay)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                if(upgradeState < maxUpgradeState && canUpgrade())
                {
                    GameResources.RemoveResourceAmount(GameResources.ResourceType.Gold, 5);
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
            childUI.gameObject.SetActive(true);
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
            childUI.gameObject.SetActive(false);
        }
    }

}
