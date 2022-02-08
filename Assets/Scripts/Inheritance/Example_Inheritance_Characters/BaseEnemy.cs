using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IEnemy_Unit, IDmg_By_Ally<int>
{

    public int health;
    private DropPool dropPool;
    private CoinPool coinPool;
    [SerializeField] private Vector3 currentPos;
    [SerializeField] private Animator anim;
    [SerializeField] private Coroutine _currentRoutine;
    [SerializeField] private Coroutine _animationRoutine;
    private bool faceR = false;
    // Start is called before the first frame update
    public virtual void Start()
    {
        health = 100;
        //dropPool = FindObjectOfType<DropPool>();
        //coinPool = FindObjectOfType<CoinPool>();
        currentPos = transform.position;
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    private void Update()
    {
        Facing();
        if (!IsArrivedFree())
        {
            transform.position = Vector2.MoveTowards(transform.position, currentPos, 5 * Time.deltaTime);
            //transform.position = Vector3.Lerp(transform.position, currentPos, Time.deltaTime * 5);
        }
        else if (IsArrivedFree())
        {
            //state = State.Base;
            //Debug.Log("arrived, should be able to change pos and be not arrived");
        }
    }
    public void GetPools()
    {
        dropPool = FindObjectOfType<DropPool>();
        coinPool = FindObjectOfType<CoinPool>();
    }
    public void SetHealthFull()
    {
        health = 100;
    }
    public void DamageTaken(int dmgTaken)
    {
        health -= dmgTaken;
    }

    private bool IsArrivedFree()//for not go to transform
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
        currentPos = new Vector3(position.x, transform.position.y);

        // and start a new routine
        _currentRoutine = StartCoroutine(WaitUntilArrivedPosition(position, OnArrivedAtPosition));

    }
    public void PlayAnimation(Action OnFinishedPlaying = null)
    {
        //do play animation in this
        //anim.Play(anim.runtimeAnimatorController.animationClips[1].name);
        if (_animationRoutine != null) StopCoroutine(_animationRoutine);
        _animationRoutine = StartCoroutine(WaitUntilFinishPlayAnimation(anim.runtimeAnimatorController.animationClips[1].length, OnFinishedPlaying));
    }

    private IEnumerator WaitUntilArrivedPosition(Vector3 position, Action OnArrivedAtPosition = null)
    {
        // yield return tells Unity "pause the routine here,
        // render this frame and continue from here in the next frame"
        // WaitWhile does what the name suggests
        // waits until the given condition is true
        yield return new WaitUntil(IsArrivedFree);

        _currentRoutine = null;
        OnArrivedAtPosition?.Invoke();
    }

    private IEnumerator WaitUntilFinishPlayAnimation(float animationClip, Action OnFinishPlayingAnimation = null)
    {
        //float animationLength = anim.length;
        //anim.Play(anim.runtimeAnimatorController.animationClips[1].name);
        anim.SetTrigger("PlayPlaceholder");
        yield return new WaitForSeconds(animationClip);
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
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
