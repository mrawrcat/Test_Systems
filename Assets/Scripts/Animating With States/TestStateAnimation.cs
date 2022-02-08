using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStateAnimation : MonoBehaviour
{
    private enum State
    {
        Idle,
        Combat_Pose,
        Run,
        Jump,
        Attack,
        Hurt,
        Death,
        Recover,
    }

    private State state;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Idle:
                anim.SetInteger("AnimState", 0);
                break;
            case State.Run:
                anim.SetInteger("AnimState", 2);
                break;
            case State.Combat_Pose:
                anim.SetInteger("AnimState", 1);
                break;
            case State.Hurt:
                anim.SetTrigger("Hurt");
                break;
        }
    }

    public void SetStateIdle()
    {
        state = State.Idle;
    }
    public void SetStateRun()
    {
        state = State.Run;
    }
    public void SetStateCombat()
    {
        state = State.Combat_Pose;
    }

    public void SetStateHurt()
    {
        state = State.Hurt;
    }
}
