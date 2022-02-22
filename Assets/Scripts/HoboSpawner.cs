using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoboSpawner : MonoBehaviour
{
    //private bool hasHobo;
    [SerializeField] private LayerMask whatIsHobo;
    [SerializeField] private Vector2 detectSize;
    private List<TaskTestHoboAI> hoboList;
    private float nextSpawnTime;
    private float spawnRate;
    private BoxCollider2D detectEnterExit;
    [SerializeField] private GameObject prefab;
    private TaskGameHandler taskHandler;
    // Start is called before the first frame update
    void Start()
    {
        detectEnterExit = GetComponent<BoxCollider2D>();
        detectEnterExit.size = detectSize;
        hoboList = new List<TaskTestHoboAI>();
        taskHandler = FindObjectOfType<TaskGameHandler>();
        nextSpawnTime = 0f;
        spawnRate = 3f;
        BaseUnit.Create_BaseUnit(transform.position, transform.position);
        
    }

    // Update is called once per frame
    void Update()
    {
        //hasHobo = Physics2D.OverlapBox(transform.position, detectSize, whatIsHobo);
        /*
        Collider2D[] hobos = Physics2D.OverlapBoxAll(transform.position, detectSize, 0, whatIsHobo);
        foreach(Collider2D hobo in hobos)
        {
            TaskTestHoboAI hoboAI = hobo.GetComponent<TaskTestHoboAI>();
            if(hoboAI != null)
            {
                hoboList.Add(hoboAI);
            }
        }
        if(hoboList.Count < 1)
        {
            //new Vector3(transform.position.x + Random.Range(-5,5), transform.position.y)
            if (Time.time > nextSpawnTime)
            {
                BaseUnit.Create_BaseUnit(transform.position, transform.position);
                nextSpawnTime = Time.time + spawnRate;
            }
        }
        */
       
        

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position, detectSize);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ally")
        {
            Collider2D[] hobos = Physics2D.OverlapBoxAll(transform.position, detectSize, 0, whatIsHobo);
            foreach (Collider2D hobo in hobos)
            {
                TaskTestHoboAI hoboAI = hobo.GetComponent<TaskTestHoboAI>();
                if (hoboAI != null)
                {
                    hoboList.Add(hoboAI);
                }
            }
        }
    }
}
