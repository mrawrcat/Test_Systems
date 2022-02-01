using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{

    private enum State
    {
        Base,
        Moving,
        InQueue,
    }
    [SerializeField]
    private float speed;
    private State state;
    private Vector3 currentPos;
    private Coroutine _currentRoutine;
    private Rigidbody2D rb2d;
    private float moveInputH, moveInputV;
    private bool faceR = true;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInputH = Input.GetAxis("Horizontal");
        moveInputV = Input.GetAxis("Vertical");
        Facing();
        //rb2d.velocity = new Vector2(moveInput *= speed, rb2d.velocity.y);
        //transform.position += transform.forward * moveInputH * speed * Time.deltaTime;
        //transform.Translate(new Vector3(moveInputH, moveInputV) * speed * Time.deltaTime);
        transform.position += new Vector3(moveInputH, moveInputV) * speed * Time.deltaTime;
        //UpdateMovement();

        if (Input.GetMouseButtonDown(0))
        {
           //MoveTo(UtilsClass.GetMouseWorldPosition());
        }
    }

    private void Flip()
    {
        faceR = !faceR;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void Facing()
    {
        if (faceR && moveInputH < 0)
        {
            Flip();
        }
        else if (!faceR && moveInputH > 0)
        {
            Flip();
        }
    }

    private bool IsArrivedFree()//for not go to transform
    {
        // Instead of setting a field directly return the value
        return Vector3.Distance(transform.position, currentPos) <= 0.0f;
    }

    private void UpdateMovement()
    {
        if (state == State.Moving)
        {
            if (!IsArrivedFree())
            {
                transform.position = Vector2.MoveTowards(transform.position, currentPos, speed * Time.deltaTime);
                //transform.position = Vector3.Lerp(transform.position, currentPos, 1 * Time.deltaTime);
            }
            else if (IsArrivedFree())
            {
                //state = State.Base;
                //Debug.Log("arrived, should be able to change pos and be not arrived");
            }

        }
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
        state = State.Moving;
        currentPos = new Vector3(position.x, position.y);

        // and start a new routine
        _currentRoutine = StartCoroutine(WaitUntilArrivedPosition(position, OnArrivedAtPosition));

    }

    public void PlayAnimation(Action OnFinishedPlaying = null)
    {
        //do play animation in this
        anim.Play("placeholder_animation");
        StartCoroutine(WaitUntilFinishPlayAnimation(anim.runtimeAnimatorController.animationClips[1], OnFinishedPlaying));


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

    private IEnumerator WaitUntilFinishPlayAnimation(AnimationClip anim, Action OnFinishPlayingAnimation = null)
    {
        //float animationLength = anim.length;
        yield return new WaitForSeconds(anim.length);
        OnFinishPlayingAnimation?.Invoke();

    }
}
