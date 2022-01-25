using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class VarmintSpawner : MonoBehaviour
{
    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave,
    }

    private State state;
    /*
    [SerializeField]
    private Text waveTxt, tilNextWaveTxt;
    [SerializeField]
    private Text gameOverWaveTxt;
    */
    public int waveNumber;
    [SerializeField]
    private float timeTilSpawn; //time in between wave spawn
    private float spawnTime;
    private EnemyPool varmintPool;

    private float nextEnemySpawnTimer;
    private int remainingSpawnAmt;
    [SerializeField]
    private int amtToSpawn;
    [SerializeField]
    private Vector2 detectBox;
    private bool spawnedEnemyDetected;
    [SerializeField]
    private LayerMask layer;
    [SerializeField]
    private bool tempWaveState;

    // Start is called before the first frame update
    void Start()
    {
        state = State.WaitingToSpawnNextWave;
        varmintPool = GetComponent<EnemyPool>();
        spawnTime = timeTilSpawn;
        waveNumber = 0;
        //amtToSpawn = 1;
        //GameObject.Find("spawn wave").GetComponent<Button_UI>().ClickFunc = () => { Debug.Log("spawn wave"); SpawnWave(); };
    }

    // Update is called once per frame
    void Update()
    {
        /*
        waveTxt.text = "Wave: " + waveNumber.ToString("F0");
        tilNextWaveTxt.text = "Next Wave: " + spawnTime.ToString("F2");
        gameOverWaveTxt.text = "You finished at Wave: " + waveNumber.ToString("F0");
        */
        spawnedEnemyDetected = Physics2D.OverlapBox((Vector2)transform.position, detectBox, 0, layer);
        Collider2D[] enemies = Physics2D.OverlapBoxAll((Vector2)transform.position, detectBox, 0, layer);

        
        switch (state)
        {
            case State.WaitingToSpawnNextWave:
                if (tempWaveState)
                {
                    spawnTime -= Time.deltaTime;
                }
                if (spawnTime <= 0 && tempWaveState)
                {
                    SpawnWave();
                }
                break;

            case State.SpawningWave:
                if (remainingSpawnAmt > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0 && !spawnedEnemyDetected)
                    {
                        ///varmintPool.SpawnEnemy(transform);
                        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1f);
                        
                        nextEnemySpawnTimer = Random.Range(.1f, .5f);
                        remainingSpawnAmt--;

                        if(remainingSpawnAmt <= 0)
                        {
                            state = State.WaitingToSpawnNextWave;
                        }
                    }
                }
                break;
        }


        

        
    }

    private void SpawnWave()
    {
        //varmintPool.SpawnVarmint(transform);
        spawnTime = timeTilSpawn;
        remainingSpawnAmt = amtToSpawn;
        state = State.SpawningWave;
        waveNumber++;
        //amtToSpawn = 5;
        //amtToSpawn = 1 + (3 * waveNumber);
        //transform.position = new Vector3(0, 0, 0);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position, detectBox);

    }
}
