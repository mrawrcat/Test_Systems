using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour, IUnit
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
    [SerializeField] private Vector3 currentPos;
    [SerializeField] private Coroutine _currentRoutine;
    [SerializeField] private Coroutine _animationRoutine;
    [SerializeField] private AnimationType activeAnimType;
    private SpriteAnimatorCustom anim;
    private bool faceR = true;

    private void Start()
    {
        currentPos = transform.position;
        anim = GetComponentInChildren<SpriteAnimatorCustom>();
        anim.SetFrameArray(idleAnim);
        activeAnimType = AnimationType.Idle;
        anim.PlayAnimationCustom(idleAnim, .1f);
    }

    private void Update()
    {
        Facing();
        if (!IsArrivedAtPosition())
        {
            transform.position = Vector2.MoveTowards(transform.position, currentPos, 5 * Time.deltaTime);
            PlayCharacterAnimation(AnimationType.Walk);
            //transform.position = Vector3.Lerp(transform.position, currentPos, Time.deltaTime * 5);
        }
        else if (IsArrivedAtPosition())
        {
            //activeAnimType = AnimationType.Idle;
            PlayCharacterAnimation(AnimationType.Idle);
        }
    }

    private bool IsArrivedAtPosition()//for not go to transform
    {
        // Instead of setting a field directly return the value
        return Vector3.Distance(transform.position, currentPos) <= 0.0f;
    }

    public void MoveTo(Vector3 position, Action OnArrivedAtPosition = null)
    {
        // Here can/have to decide
        // Either Option A
        // do not allow a new move call if there is already one running
        //if (_currentRoutine != null) return;
        // OR Option B
        // interrupt the current routine and start a new one
        if (_currentRoutine != null) StopCoroutine(_currentRoutine);

        // Set the destination directly
        //state = State.NotInQueue_Free;
        currentPos = new Vector3(position.x, position.y);

        // and start a new routine
        _currentRoutine = StartCoroutine(WaitUntilArrivedPosition(position, OnArrivedAtPosition));

    }
    public void PlayAnimation(float animLength, Action OnFinishedPlaying = null)
    {
        //do play animation in this
        //anim.Play(animClip.name);
        if (_animationRoutine != null) StopCoroutine(_animationRoutine);
        _animationRoutine = StartCoroutine(WaitUntilFinishPlayAnimation(animLength, OnFinishedPlaying));
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

    private IEnumerator WaitUntilArrivedPosition(Vector3 position, Action OnArrivedAtPosition = null)
    {
        // yield return tells Unity "pause the routine here,
        // render this frame and continue from here in the next frame"
        // WaitWhile does what the name suggests
        // waits until the given condition is true
        yield return new WaitUntil(IsArrivedAtPosition);

        _currentRoutine = null;
        OnArrivedAtPosition?.Invoke();
    }

    private IEnumerator WaitUntilFinishPlayAnimation(float secElapsed, Action OnFinishPlayingAnimation = null)
    {
        yield return new WaitForSeconds(secElapsed);
        _animationRoutine = null;
        OnFinishPlayingAnimation?.Invoke();
    }
    private void Facing()
    {
        if (faceR && currentPos.x < transform.position.x)
        {
            Flip();
        }
        else if (!faceR && currentPos.x > transform.position.x)
        {
            Flip();
        }
    }
    private void Flip()
    {
        faceR = !faceR;
        //Vector3 scaler = transform.localScale;
        //scaler.x *= -1;
        //transform.localScale = scaler;
        Vector3 spriteScaler = GetComponentInChildren<SpriteRenderer>().transform.localScale;
        spriteScaler.x *= -1;
        GetComponentInChildren<SpriteRenderer>().transform.localScale = spriteScaler;

    }
}
