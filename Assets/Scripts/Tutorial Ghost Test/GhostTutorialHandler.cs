using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTutorialHandler : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform townCenter;
    [SerializeField] private Transform builderStation;
    [SerializeField] private Transform archerStation;
    [SerializeField] private GameObject ghost;

    public event EventHandler On_Made_Campfire;
    public event EventHandler On_Made_Archer;
    public event EventHandler On_Made_Builder;


    private GameObject ghostSave;
    private Tutorial_Ghost tutorial_Ghost;
    private TaskSystem<TaskGameHandler.GhostTask> ghostTaskSytem;

    private bool madeCampfire;
    private bool madeOneArcher;
    private bool madeOneBuilder;

    
    // Start is called before the first frame update
    void Start()
    {
        ghostTaskSytem = new TaskSystem<TaskGameHandler.GhostTask>();
        GameObject spawnedWorker = Instantiate(ghost);
        spawnedWorker.transform.position = player.transform.position + new Vector3(3, 0); //ghost in kingdom spawns at monument which you have to pass
        spawnedWorker.GetComponent<GhostTutorial_TaskSystemAI>().SetUp(spawnedWorker.GetComponent<Tutorial_Ghost>(), ghostTaskSytem);
        ghostSave = spawnedWorker;
        tutorial_Ghost = ghostSave.GetComponent<Tutorial_Ghost>();
        tutorial_Ghost.onPlayerTriggerEnter += ghost_Touched_Player_Event;
        On_Made_Campfire += On_Made_Campfire_Event;
        On_Made_Archer += On_Made_Archer_Event;
        On_Made_Builder += On_Made_Builder_Event;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private bool isNearPlayer()
    {
        return Vector3.Distance(transform.position, player.position) <= 5.0f;
    }
    private void ghost_Touched_Player_Event(object sender, EventArgs e)
    {
        Debug.Log("Ghost touched player from handler");
       
        TaskGameHandler.GhostTask task = new TaskGameHandler.GhostTask.MoveToPosition { targetPosition = new Vector3(townCenter.position.x + 2f, -3f) };
        ghostTaskSytem.AddTask(task);
        tutorial_Ghost.onPlayerTriggerEnter -= ghost_Touched_Player_Event; //finished tutorial so dont have to touch ghost anymore so unsubscribe to event
           
        /*
        if (!madeOneArcher)
        {
            TaskGameHandler.GhostTask task = new TaskGameHandler.GhostTask.MoveToPosition { targetPosition = new Vector3(archerStation.position.x +2f, -3f) };
            ghostTaskSytem.AddTask(task);
        }
        else if (!madeOneBuilder)
        {
            TaskGameHandler.GhostTask task = new TaskGameHandler.GhostTask.MoveToPosition { targetPosition = new Vector3(builderStation.position.x + 2f, -3f) };
            ghostTaskSytem.AddTask(task);
        }
        */

    }

    private void On_Made_Campfire_Event(object sender, EventArgs e)
    {
        //go to player? direct him to archer station?
        TaskGameHandler.GhostTask task = new TaskGameHandler.GhostTask.MoveToPosition { targetPosition = new Vector3(archerStation.position.x + 2f, -3f) };
        ghostTaskSytem.AddTask(task);
    }
    private void On_Made_Archer_Event(object sender, EventArgs e)
    {
        //go to player? direct him to builder station?
        TaskGameHandler.GhostTask task = new TaskGameHandler.GhostTask.MoveToPosition { targetPosition = new Vector3(builderStation.position.x + 2f, -3f) };
        ghostTaskSytem.AddTask(task);
    }
    private void On_Made_Builder_Event(object sender, EventArgs e)
    {
        ghostSave.SetActive(false); //hide ghost -> ideally ghost fades out
    }
    

    public bool GetCampfire()
    {
        return madeCampfire;
    } 
    public bool GetArcher()
    {
        return madeOneArcher;
    } 
    public bool GetBuilder()
    {
        return madeOneBuilder;
    }


    public void Set_Made_Campfire()
    {
        madeCampfire = true;
        On_Made_Campfire?.Invoke(this, EventArgs.Empty);
    }
    public void Set_One_Archer()
    {
        madeOneArcher = true;
        On_Made_Archer?.Invoke(this, EventArgs.Empty);
    }
    public void Set_One_Builder()
    {
        madeOneBuilder = true;
        On_Made_Builder?.Invoke(this, EventArgs.Empty);
    }
}
