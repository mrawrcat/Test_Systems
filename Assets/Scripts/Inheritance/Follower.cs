using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour, IWorker
{

    private enum State
    {
        Base,
        NotInQueue_Free,
        InQueue,
    }
    private enum QueuedState
    {
        Idle,
        InQueue,
        InBuilding,

    }
    [SerializeField]
    private Sprite tempWorkerSprite;
    [SerializeField]
    private State state;
    private QueuedState queuedState;
    
    private Rigidbody2D rb2d;
    [SerializeField]
    private Vector3 currentPos;
    private Coroutine _currentRoutine;
    private Coroutine _animationRoutine;
    private float inverseMoveTime;
    private Transform currentTransform;
    private Animator anim;
    public Sprite GetWorkerSprite()
    {
        return tempWorkerSprite;
    }
    // Start is called before the first frame update
    void Start()
    {
        //GameHandler.villagersInScene.Add(gameObject);
        state = State.Base;
        queuedState = QueuedState.Idle;
        currentPos = transform.position;
        inverseMoveTime = 1 / .1f;
        //player = FindObjectOfType<PlayerMovement>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //Debug.Log(anim.runtimeAnimatorController.animationClips[1].name);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.InQueue)
        {
            if (!IsArrived())//in queue
            {

                //StartCoroutine(SmoothMovement(currentTransform.position));
                transform.position = Vector2.MoveTowards(transform.position, currentTransform.position, 5 * Time.deltaTime);
                //transform.position = Vector3.Lerp(transform.position, currentTransform.position, Time.deltaTime * 5);
                //transform.position = Vector2.MoveTowards(transform.position, currentPos, 5 * Time.deltaTime);
                //transform.Translate(dir * Time.deltaTime);
                //rb2d.velocity = new Vector3(dir.x, 0, 0);
            }
            else if (IsArrived())
            {
                //state = State.Idle;
                //state = State.NotInQueue_Free;
            }
        }
        if (state == State.NotInQueue_Free)
        {



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

        if (Input.GetKeyDown(KeyCode.P))
        {
            //anim.Play(anim.runtimeAnimatorController.animationClips[1].name);
            //anim.SetTrigger("PlayPlaceholder");
        }
    }


    private bool IsArrived()//for go to transform
    {
        // Instead of setting a field directly return the value
        return Vector3.Distance(transform.position, currentTransform.position) <= 0.0f;
    }
    private bool IsArrivedFree()//for not go to transform
    {
        // Instead of setting a field directly return the value
        return Vector3.Distance(transform.position, currentPos) <= 0.0f;
    }

    public void MoveToTransform(Transform position, Action OnArrivedAtPosition = null)//for moving queue
    {


        // Here can/have to decide
        // Either Option A
        // do not allow a new move call if there is already one running
        //if (_currentRoutine != null) return;
        // OR Option B
        // interrupt the current routine and start a new one
        if (_currentRoutine != null) StopCoroutine(_currentRoutine);

        // Set the destination directly

        state = State.InQueue;
        currentTransform = position;
        //StartCoroutine(SmoothMovement(currentTransform.position));
        // and start a new routine
        _currentRoutine = StartCoroutine(WaitUntilArrivedTransformPosition(position.position, OnArrivedAtPosition));

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
        state = State.NotInQueue_Free;
        currentPos = new Vector3(position.x, position.y);

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

    private IEnumerator WaitUntilArrivedTransformPosition(Vector3 position, Action OnArrivedAtPosition = null)
    {
        // yield return tells Unity "pause the routine here,
        // render this frame and continue from here in the next frame"
        // WaitWhile does what the name suggests
        // waits until the given condition is true
        yield return new WaitUntil(IsArrived);

        _currentRoutine = null;
        OnArrivedAtPosition?.Invoke();
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

    public void SetQueuedStateIdle()
    {
        queuedState = QueuedState.Idle;
    }

    public void SetQueuedStateQueued()
    {
        queuedState = QueuedState.InQueue;
    }

    public bool IsQueued()
    {
        return queuedState == QueuedState.InQueue;
    }

    public void setCurrentPos(Vector3 position)
    {
        currentPos = position;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainDist = (transform.position - end).sqrMagnitude;

        while (sqrRemainDist > float.Epsilon)
        {
            Vector3 newPos = Vector3.MoveTowards(rb2d.position, end, inverseMoveTime * 5 * Time.deltaTime);
            rb2d.MovePosition(newPos);
            sqrRemainDist = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }


}
