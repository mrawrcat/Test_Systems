using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class OrbitHandler : MonoBehaviour
{
    [SerializeField] private Sprite followerSprite;
    [SerializeField] private RuntimeAnimatorController animatorController;
    [SerializeField] private Transform orbitTransform;
    private List<Follower> followerList;
    private float radius = 1;
    private int currentFollowers;
    private int followersRequireForFullCircle = 30;
    private TaskSystem taskSystem;
    // Start is called before the first frame update
    void Start()
    {
        taskSystem = new TaskSystem();
        followerList = new List<Follower>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //spawn a follower
            SpawnFollower(UtilsClass.GetMouseWorldPosition());
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //TaskSystem.Task task = new TaskSystem.Task.MoveToPosition { targetPosition = orbitTransform.position + new Vector3(5, 5) };
            //taskSystem.AddTask(task);
            //followerList[0].GetComponent<Follower>().MoveToTransform(orbitTransform);
            //followerList[0].gameObject.transform.SetParent(orbitTransform);
            for(int i = 0; i < followerList.Count; i++)
            {
                if(followerList[i].transform.parent != orbitTransform)
                {
                    followerList[i].GetComponent<Follower>().MoveTo(GetPositon(orbitTransform));
                    followerList[i].gameObject.transform.SetParent(orbitTransform);
                    currentFollowers++;
                    break;
                }
            }
        }
    }

    public Vector3 GetPositon(Transform objectPos)
    {
        float angle = currentFollowers * Mathf.PI * 2f / followersRequireForFullCircle;
        return objectPos.position + new Vector3(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius, 0);
    }

    private void SpawnFollower(Vector3 position)
    {
        GameObject followerObj = FollowerObj(position);
        followerList.Add(followerObj.GetComponent<Follower>());
        Debug.Log(followerList.Count);
    }


    private GameObject FollowerObj(Vector3 position)
    {
        GameObject gameObject = new GameObject("Follower", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = followerSprite;
        gameObject.transform.position = position;
        gameObject.transform.SetParent(transform);
        gameObject.AddComponent<Follower>();
        gameObject.AddComponent<Animator>();
        gameObject.GetComponent<Animator>().runtimeAnimatorController = animatorController;
        gameObject.AddComponent<TaskWorkerAI>();
        gameObject.GetComponent<TaskWorkerAI>().SetUp(gameObject.GetComponent<Follower>(), taskSystem);
        return gameObject;
    }
}
