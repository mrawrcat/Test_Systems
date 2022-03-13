using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallyVillagers : MonoBehaviour
{

    [SerializeField]private bool foundVillager;
    [SerializeField]
    private LayerMask whatIsVillager;
    [SerializeField]
    private Transform atkPos;
    [SerializeField]
    private Vector2 atkBoxSize;
    [SerializeField]
    private float circleSize;
    private QueueGameHandler gameHandler;
    private TestTransformHandler testHandler;
    private OrbitHandler orbitHandler;
    // Start is called before the first frame update
    void Start()
    {
        gameHandler = FindObjectOfType<QueueGameHandler>();
        testHandler = FindObjectOfType<TestTransformHandler>();
        orbitHandler = FindObjectOfType<OrbitHandler>();
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
            //testHandler.SendGuest();
        }
        foundVillager = Physics2D.OverlapCircle((Vector2)atkPos.position, circleSize, 0, whatIsVillager);
        //Collider2D[] villagers = Physics2D.OverlapCircleAll((Vector2)atkPos.position, circleSize, 0, whatIsVillager);
        if (foundVillager)
        {
            Debug.Log("follower in circle");
        }
        
    }

    private void RallySkill()
    {
        //foundVillager = Physics2D.OverlapBox((Vector2)atkPos.position, atkBoxSize, 0, whatIsVillager);
        Collider2D[] villagers = Physics2D.OverlapBoxAll((Vector2)atkPos.position, atkBoxSize, 0, whatIsVillager);
        foreach (Collider2D villager in villagers)
        {
            //villager.GetComponent<Villager>().SetStateToFollow();
            gameHandler.DoAddGuest(villager.GetComponent<Villager>());
            //testHandler.DoAddGuest(villager.GetComponent<Villager>());
            //orbitHandler.DoAddGuest(villager.GetComponent<Follower>());
            Debug.Log("found follwer");
            return;
        }
    }
    /*
    */

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)atkPos.position, circleSize);
        Gizmos.DrawWireCube((Vector2)atkPos.position, atkBoxSize);

    }
}
