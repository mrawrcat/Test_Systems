using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTutorial : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform townCenter;
    [SerializeField] private Transform builderStation;
    [SerializeField] private Transform archerStation;
    [SerializeField] private GameObject ghost;
    private bool isNearPlayer()
    {
        return Vector3.Distance(transform.position, player.position) <= 10.0f;
    }
    private TaskSystem<TaskGameHandler.GhostTask> ghostTaskSytem;
    // Start is called before the first frame update
    void Start()
    {
        ghostTaskSytem = new TaskSystem<TaskGameHandler.GhostTask>();

        GameObject spawnedWorker = Instantiate(ghost);
        spawnedWorker.transform.position = player.transform.position + new Vector3(-3, 0); //doesnt have to be middle just make it spawn near player
        spawnedWorker.GetComponent<GhostTutorial_TaskSystemAI>().SetUp(spawnedWorker.GetComponent<Tutorial_Ghost>(), ghostTaskSytem);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
