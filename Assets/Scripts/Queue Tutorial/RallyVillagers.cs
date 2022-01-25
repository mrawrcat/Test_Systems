using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallyVillagers : MonoBehaviour
{

    private bool foundVillager;
    [SerializeField]
    private LayerMask whatIsVillager;
    [SerializeField]
    private Transform atkPos;
    [SerializeField]
    private Vector2 atkBoxSize;
    private GameHandler gameHandler;
    private TestHandler testHandler;
    private List<Villager> toRally = new List<Villager>(); 
    // Start is called before the first frame update
    void Start()
    {
        gameHandler = FindObjectOfType<GameHandler>();
        testHandler = FindObjectOfType<TestHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RallySkill();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            //gameHandler.SendGuest();
            testHandler.SendGuest();
        }
    }

    private void RallySkill()
    {
        foundVillager = Physics2D.OverlapBox((Vector2)atkPos.position, atkBoxSize, 0, whatIsVillager);
        Collider2D[] villagers = Physics2D.OverlapBoxAll((Vector2)atkPos.position, atkBoxSize, 0, whatIsVillager);

        if (foundVillager)
        {
            //Debug.Log(villagers.Length);
            foreach (Collider2D villager in villagers)
            {
                /*
                    add them into a list or dictionary, make them follow player
                    maybe for follow best to make villager have 2 or 3 states -> set state to follow -> follow the guy in front? follow player at random length?
                */
                //toRally.Add(villager.GetComponent<Villager>());
                //villager.GetComponent<Villager>().SetStateToFollow();
                //gameHandler.DoAddGuest(villager.GetComponent<Villager>());
                testHandler.DoAddGuest(villager.GetComponent<Villager>());
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)atkPos.position, atkBoxSize);

    }
}
