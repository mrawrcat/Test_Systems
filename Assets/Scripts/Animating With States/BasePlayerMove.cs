using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerMove : MonoBehaviour
{
    private BasePlayer basePlayer;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        basePlayer = GetComponent<BasePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(basePlayer.GetMoveInput(), 0) * speed * Time.deltaTime;
        if(basePlayer.GetMoveInput() < 0)
        {
            //move left
        }
        else if(basePlayer.GetMoveInput() > 0)
        {
            //move right
        }
    }
}
