using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IEnemy_Unit, IDmg_By_Ally<int>
{
    public static void Create_BaseUnit(Vector3 spawnPos, Vector3 targetPos)
    {
        Transform baseEnemyTransform = Instantiate(GameResources.instance.Bandit, spawnPos, Quaternion.identity);
        BaseEnemy baseEnemy = baseEnemyTransform.GetComponent<BaseEnemy>();
        baseEnemy.SetUp(targetPos);
    }

    private static List<BaseEnemy> activeBaseEnemyList;

    public static BaseEnemy GetClosestEnemy(Vector3 position, float maxRange)
    {
        BaseEnemy closest = null;
        foreach(BaseEnemy baseEnemy in activeBaseEnemyList)
        {
            if(closest == null)
            {
                closest = baseEnemy;
            }
            else
            {
                float currentClosest = Vector3.Distance(position, closest.transform.position);
                float currentChecking = Vector3.Distance(position, baseEnemy.transform.position);
                if (currentChecking < currentClosest)
                {
                    closest = baseEnemy;
                }
            }
        }
        return closest;
    }

    public void RemoveThisFromActiveBaseEnemyList()
    {
        activeBaseEnemyList.RemoveAt(GetIndexPositionInActiveBaseEnemyList());
    }

    public int GetIndexPositionInActiveBaseEnemyList()
    {
        return activeBaseEnemyList.IndexOf(this);
    }

    public enum AnimationType
    {
        Idle,
        Walk,
        Attack,
        Hurt,
        Die,
    }
    public AnimationType activeAnimType;
    public AnimationType GetActiveAnimType()
    {
        return activeAnimType;
    }

    [SerializeField] private Sprite[] idleAnim;
    [SerializeField] private Sprite[] walkAnim;
    [SerializeField] private Sprite[] atkAnim;
    [SerializeField] private Sprite[] hurtAnim;
    [SerializeField] private Sprite[] dieAnim;
    private SpriteAnimatorCustom anim;

    public int health;
    private DropPool dropPool;
    private CoinPool coinPool;
    [SerializeField] private Vector3 currentPos;
    [SerializeField] private Coroutine _currentRoutine;
    [SerializeField] private Coroutine _animationRoutine;
    private bool faceR = false;
   

    private void SetUp(Vector3 targetPos)
    {
        this.currentPos = targetPos;
    }

    private void Awake()
    {
        if(activeBaseEnemyList == null)
        {
            activeBaseEnemyList = new List<BaseEnemy>();
        }
        activeBaseEnemyList.Add(this);
        //Debug.Log(activeBaseEnemyList.Count);
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        health = 100;
        //dropPool = FindObjectOfType<DropPool>();
        //coinPool = FindObjectOfType<CoinPool>();
        //currentPos = transform.position;
        anim = GetComponentInChildren<SpriteAnimatorCustom>();
        anim.SetFrameArray(idleAnim);
        activeAnimType = AnimationType.Idle;
        anim.PlayAnimationCustom(idleAnim, .1f);

    }

    // Update is called once per frame
    private void Update()
    {
        
        Facing();
        if (!IsArrivedAtPosition())
        {
            transform.position = Vector2.MoveTowards(transform.position, currentPos, 5 * Time.deltaTime);
            //transform.position = Vector3.Lerp(transform.position, currentPos, Time.deltaTime * 5);
        }
        else if (IsArrivedAtPosition())
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
        if(health <= 0)
        {
            RemoveThisFromActiveBaseEnemyList();
            Destroy(gameObject);
            //gameObject.SetActive(false);
        }
    }

    public bool IsArrivedAtPosition()//for not go to transform
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
    public void PlayAnimation(float animLength, Action OnFinishedPlaying = null)
    {
        //do play animation in this
        //anim.Play(animClip.name);
        if (_animationRoutine != null) StopCoroutine(_animationRoutine);
        _animationRoutine = StartCoroutine(WaitUntilFinishPlayAnimation(animLength, OnFinishedPlaying));
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

    private IEnumerator WaitUntilFinishPlayAnimation(float animationClip, Action OnFinishPlayingAnimation = null)
    {
        //float animationLength = anim.length;
        //anim.Play(anim.runtimeAnimatorController.animationClips[1].name);
        yield return new WaitForSeconds(animationClip);
        _animationRoutine = null;
        OnFinishPlayingAnimation?.Invoke();

    }

    public void PlayCharacterAnimation(AnimationType animType)
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
