using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimatorCustom : MonoBehaviour
{
    public event EventHandler OnAnimationFrameCounterIncrease;
    public event EventHandler OnAnimationFrameCounterAlmostFinished;
    public event EventHandler OnAnimationLoopedFirstTime;
    public event EventHandler OnAnimationLooped;
    public event EventHandler OnAnimationLoopedStopPlaying;
    [SerializeField]private Sprite[] frameArray;
    private int currentFrame;
    private float timer;
    private float frameRate;
    private bool loop = false;
    [SerializeField] private bool isPlaying = true;
    private int loopCounter = 0;
    private SpriteRenderer spriteRenderer;
    public bool GetIsPlayingBool()
    {
        return isPlaying;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        /*
        if (frameArray != null)
        {
            PlayAnimationCustom(frameArray, frameRate);
        }
        else
        {
            isPlaying = false;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
        {
            return;
        }
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % frameArray.Length;
            OnAnimationFrameCounterIncrease?.Invoke(this, EventArgs.Empty);
            if (!loop && currentFrame == 0)
            {
                StopPlaying();
            }
            else
            {
                spriteRenderer.sprite = frameArray[currentFrame];
            }

            if (currentFrame == 0)
            {
                loopCounter++;
                if (loopCounter == 1)
                {
                    OnAnimationLoopedFirstTime?.Invoke(this, EventArgs.Empty);
                    //if (OnAnimationLoopedFirstTime != null) OnAnimationLoopedFirstTime(this, EventArgs.Empty);
                }
                OnAnimationLooped?.Invoke(this, EventArgs.Empty);
                //if (OnAnimationLooped != null) OnAnimationLooped(this, EventArgs.Empty);
            }
            if(currentFrame == Mathf.RoundToInt(frameArray.Length * .7f))
            {
                OnAnimationFrameCounterAlmostFinished?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void StopPlaying()
    {
        isPlaying = false;
        OnAnimationLoopedStopPlaying?.Invoke(this, EventArgs.Empty);
    }



    public void SetFrameArray(Sprite[] frameArray)
    {
        this.frameArray = frameArray;
    }

    public void PlayAnimationCustom(Sprite[] frameArray, float frameRate, bool loop = true)
    {
        this.frameArray = frameArray;
        this.frameRate = frameRate;
        this.loop = loop;
        isPlaying = true;
        currentFrame = 0;
        timer = 0;
        spriteRenderer.sprite = frameArray[currentFrame];
    }
}
