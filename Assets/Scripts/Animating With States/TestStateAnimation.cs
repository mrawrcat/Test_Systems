using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStateAnimation : MonoBehaviour
{
    private enum AnimationType
    {
        Idle,
        Walk,
        Attack,
        Hurt,
        Die,
    }
    [SerializeField] private Sprite[] idleAnim;
    [SerializeField] private Sprite[] walkAnim;
    [SerializeField] private Sprite[] atkAnim;
    [SerializeField] private Sprite[] hurtAnim;
    [SerializeField] private Sprite[] dieAnim;
    [SerializeField] private AnimationType activeAnimType;
    private SpriteAnimatorCustom anim;
    private bool isIdle;

    private void Start()
    {
        anim = GetComponentInChildren<SpriteAnimatorCustom>();
        anim.OnAnimationLoopedFirstTime += OnAnimationLooped_SingleLoop;
        anim.OnAnimationLooped += OnAnimationLooped_ContinuousLoop;
        anim.OnAnimationLoopedStopPlaying += OnAnimationLooped_StoppedPlaying;
        anim.SetFrameArray(idleAnim);
        activeAnimType = AnimationType.Idle;
        anim.PlayAnimationCustom(idleAnim, .1f);
    }

    private void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayCharacterAnimation(AnimationType.Attack);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayCharacterAnimation(AnimationType.Idle);
        }
        
        
    }

    private IEnumerator AttackToIdle()
    {
        isIdle = false;
        PlayCharacterAnimation(AnimationType.Attack);
        yield return new WaitForSeconds(3f);
        isIdle = true;
    }

    private void Walk()
    {
        PlayCharacterAnimation(AnimationType.Walk);
    }

    private void OnAnimationLooped_SingleLoop(object sender, EventArgs e)
    {
        Debug.Log("animation played once");
    }

    private void OnAnimationLooped_ContinuousLoop(object sender, EventArgs e)
    {
        Debug.Log("animation playing multiple times");
    }

    private void OnAnimationLooped_StoppedPlaying(object sender, EventArgs e)
    {
        if (activeAnimType == AnimationType.Attack)
        {
            Debug.Log("animationType is attack, go to idle");
            PlayCharacterAnimation(AnimationType.Idle);
        }
    }

    private void PlayCharacterAnimation(AnimationType animType)
    {
        if (animType != activeAnimType)
        {
            activeAnimType = animType;
            switch (animType)
            {
                default:
                case AnimationType.Idle:
                    //spriteAnim.SetFrameArray(idleAnim);
                    anim.PlayAnimationCustom(idleAnim, .1f);
                    break;
                case AnimationType.Walk:
                    //spriteAnim.SetFrameArray(walkAnim);
                    anim.PlayAnimationCustom(walkAnim, .1f);
                    break;
                case AnimationType.Attack:
                    //spriteAnim.SetFrameArray(walkAnim);
                    anim.PlayAnimationCustom(atkAnim, .1f, false);
                    break;
                case AnimationType.Hurt:
                    //spriteAnim.SetFrameArray(walkAnim);
                    anim.PlayAnimationCustom(hurtAnim, .1f, false);
                    break;
                case AnimationType.Die:
                    //spriteAnim.SetFrameArray(walkAnim);
                    anim.PlayAnimationCustom(dieAnim, .1f, false);
                    break;
            }
        }
    }
}
