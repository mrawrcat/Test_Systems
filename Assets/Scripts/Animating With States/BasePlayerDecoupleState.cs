using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerDecoupleState : MonoBehaviour
{
    private enum State
    {
        Idle,
        Walk,
        Attack,
        AttackingMode,
        FearRun,
        Cower,
        Hurt,
        Die,
    }
    [SerializeField] private State state;
    private SpriteAnimatorCustom anim;
    private BasePlayer basePlayer;
    private void Start()
    {
        basePlayer = GetComponent<BasePlayer>();
        anim = GetComponentInChildren<SpriteAnimatorCustom>();
        anim.OnAnimationFrameCounterIncrease += OnAnimationFrameCounterIncrease_DoStuff;
        anim.OnAnimationLoopedStopPlaying += OnAnimationLooped_StopPlaying;
        anim.OnAnimationLooped += OnAnimationLooped_Looped;
        anim.OnAnimationLoopedFirstTime += OnAnimationLooped_LoopedFirstTime;
    }


    private void OnAnimationFrameCounterIncrease_DoStuff(object sender, EventArgs e)
    {
        if (basePlayer.GetMoveInput() != 0)
        {
            state = State.Walk;
            basePlayer.PlayCharacterAnimation(BasePlayer.AnimationType.Walk);
        }
        else if (basePlayer.GetMoveInput() == 0)
        {
            state = State.Idle;
            basePlayer.PlayCharacterAnimation(BasePlayer.AnimationType.Idle);
        }
        
    }

    private void OnAnimationLooped_LoopedFirstTime(object sender, EventArgs e)
    {
       
    }

    private void OnAnimationLooped_Looped(object sender, EventArgs e)
    {
        
    }

    private void OnAnimationLooped_StopPlaying(object sender, EventArgs e)
    {
        //for the hit and death animation -> go back to walk/idle
    }
}
