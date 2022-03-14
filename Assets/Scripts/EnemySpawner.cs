using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    private enum State
    {
        WaitingToSpawnNextWave,
        SpawningWave,
    }
    private State state;
    [SerializeField] private int waveNumber;
    private float timeTilSpawn; //time in between wave spawn
    private float spawnTime;
    private float nextEnemySpawnTimer;
    [SerializeField] private int remainingSpawnAmt;
    private int amtToSpawn;
    [SerializeField] private int startSpawnAmt;
    [SerializeField] private Vector2 randomSpawnTimerValues;
    private bool canSpawnWaves;
    private KingdomDayNightCycle kingdomCycle;
    // Start is called before the first frame update
    void Start()
    {
        state = State.WaitingToSpawnNextWave;
        spawnTime = timeTilSpawn;
        waveNumber = 0;
        amtToSpawn = startSpawnAmt;
        canSpawnWaves = false;
        kingdomCycle = FindObjectOfType<KingdomDayNightCycle>();
        kingdomCycle.OnDawnStart += DayStart_StopSpawn;
        kingdomCycle.OnNightStart += NightStart_StartSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateWaveState();
        
    }

    private void CalculateWaveState()
    {
        switch (state)
        {
            case State.WaitingToSpawnNextWave:
                if (spawnTime > 0 && canSpawnWaves)
                {
                    spawnTime -= Time.deltaTime;
                }
                else if (spawnTime <= 0 && canSpawnWaves)
                {
                    SpawnWave();
                }
                break;
            case State.SpawningWave:
                if (remainingSpawnAmt > 0)
                {
                    nextEnemySpawnTimer -= Time.deltaTime;
                    if (nextEnemySpawnTimer < 0)
                    {
                        BaseEnemy.Create_BaseUnit(transform.position, new Vector3(0, 0), true);
                        nextEnemySpawnTimer = Random.Range(randomSpawnTimerValues.x, randomSpawnTimerValues.y);
                        remainingSpawnAmt--;

                        if (remainingSpawnAmt <= 0)
                        {
                            state = State.WaitingToSpawnNextWave;
                            canSpawnWaves = false;  
                        }
                    }
                }
                break;
        }
    }

    private void SpawnWave()
    {
        //BaseEnemy.Create_BaseUnit(transform.position, new Vector3(0, 0), true);
        spawnTime = timeTilSpawn;
        remainingSpawnAmt = amtToSpawn;
        state = State.SpawningWave;
        waveNumber++;
        amtToSpawn = 1 + (3 * waveNumber);
        
    }

    private void NightStart_StartSpawn(object sender, System.EventArgs e)
    {
        canSpawnWaves = true;
    }
    private void DayStart_StopSpawn(object sender, System.EventArgs e)
    {
        canSpawnWaves = false; 
    }
}
