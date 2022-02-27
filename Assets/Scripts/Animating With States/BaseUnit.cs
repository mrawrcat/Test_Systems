using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour, IUnit, IDamagable<int>
{
    public static void Create_BaseUnit(Vector3 spawnPos, Vector3 startingRoamPos, BaseUnit.UnitType createUnitType, Vector3 targetPos, bool roam = false)
    {
        Transform baseUnitTransform = Instantiate(GameResources.instance.AllyUnit, spawnPos, Quaternion.identity);
        BaseUnit baseUnit = baseUnitTransform.GetComponent<BaseUnit>();       
        baseUnit.SetUp(startingRoamPos, createUnitType, targetPos, roam);
        
    }
    public static void Create_BaseUnit(Vector3 spawnPos, Vector3 startingRoamPos, BaseUnit.UnitType createUnitType, bool roam = false)
    {
        Transform baseUnitTransform = Instantiate(GameResources.instance.AllyUnit, spawnPos, Quaternion.identity);
        BaseUnit baseUnit = baseUnitTransform.GetComponent<BaseUnit>();       
        baseUnit.SetUp(startingRoamPos, createUnitType, roam);
    }
    

    private TaskGameHandler taskGameHandler;
    

    private void SetUp(Vector3 startingRoamPos, UnitType onCreateUnitType, bool roam = false)
    {
        taskGameHandler = FindObjectOfType<TaskGameHandler>();

        //base unit starts as hobo
        TaskTestHoboAI hoboAI = gameObject.GetComponent<TaskTestHoboAI>();
        if (roam == true)
        {
            hoboAI.SetUp(startingRoamPos);
        }
        else
        {
            hoboAI.SetUp(new Vector3(0, 5));
        }

        TaskTestVillagerAI villagerAI = gameObject.GetComponent<TaskTestVillagerAI>();
        villagerAI.SetUp(gameObject.GetComponent<BaseUnit>(), taskGameHandler.villagerTaskSystem);
        //villagerAI.enabled = false;
        TaskTestNewWorkerAI testNewWorker = gameObject.GetComponent<TaskTestNewWorkerAI>();
        testNewWorker.SetUp(gameObject.GetComponent<BaseUnit>(), taskGameHandler.testTaskSystem);
        //testNewWorker.enabled = false;
        
        if(onCreateUnitType == UnitType.Hobo)
        {
            villagerAI.enabled = false;
            testNewWorker.enabled = false;
        }
        else if (onCreateUnitType == UnitType.Villager)
        {
            hoboAI.enabled = false;
            villagerAI.enabled = true;
            testNewWorker.enabled = false;
        }
        else if(onCreateUnitType == UnitType.Archer)
        {
            hoboAI.enabled = false;
            villagerAI.enabled = false;
            testNewWorker.enabled = true;
        }
        currentPos = transform.position;
    }

    private void SetUp(Vector3 startingRoamPos, UnitType onCreateUnitType, Vector3 targetPos, bool roam = false)
    {
        taskGameHandler = FindObjectOfType<TaskGameHandler>();

        //base unit starts as hobo
        TaskTestHoboAI hoboAI = gameObject.GetComponent<TaskTestHoboAI>();
        if(roam == true)
        {
            hoboAI.SetUp(startingRoamPos);
        }
        else
        {
            hoboAI.SetUp(new Vector3(0, 5));
        }

        TaskTestVillagerAI villagerAI = gameObject.GetComponent<TaskTestVillagerAI>();
        villagerAI.SetUp(gameObject.GetComponent<BaseUnit>(), taskGameHandler.villagerTaskSystem);
        
        TaskTestNewWorkerAI testNewWorker = gameObject.GetComponent<TaskTestNewWorkerAI>();
        testNewWorker.SetUp(gameObject.GetComponent<BaseUnit>(), taskGameHandler.testTaskSystem);
       

        if (onCreateUnitType == UnitType.Hobo)
        {
            villagerAI.enabled = false;
            testNewWorker.enabled = false;
        }
        else if (onCreateUnitType == UnitType.Villager)
        {
            hoboAI.enabled = false;
            villagerAI.enabled = true;
            testNewWorker.enabled = false;
        }
        else if (onCreateUnitType == UnitType.Archer)
        {
            hoboAI.enabled = false;
            villagerAI.enabled = false;
            testNewWorker.enabled = true;
        }
        currentPos = targetPos;
    }

    private static List<BaseUnit> activeBaseUnitList;
    public static BaseUnit GetClosestEnemy(Vector3 position, float maxRange)
    {
        BaseUnit closest = null;
        foreach (BaseUnit baseUnit in activeBaseUnitList)
        {
            if (closest == null)
            {
                closest = baseUnit;
            }
            else
            {
                float currentClosest = Vector3.Distance(position, closest.transform.position);
                float currentChecking = Vector3.Distance(position, baseUnit.transform.position);
                if (currentChecking < currentClosest)
                {
                    closest = baseUnit;
                }
            }
        }
        return closest;
    }

    public void RemoveThisFromActiveBaseEnemyList()
    {
        activeBaseUnitList.RemoveAt(GetIndexPositionInActiveBaseUnitList());
    }

    public int GetIndexPositionInActiveBaseUnitList()
    {
        return activeBaseUnitList.IndexOf(this);
    }

    public Vector3 GetMoveToCurrentPos()
    {
        return currentPos;
    }

    public enum UnitType
    {
        Hobo,
        Villager,
        Archer,
        Builder,
    }
    public UnitType unitType;

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

    //just make animationType public and move state to another script?

    public event EventHandler OnTakeDamage;
    public event EventHandler OnKilled;

    [SerializeField] private Sprite[] idleAnim;
    [SerializeField] private Sprite[] walkAnim;
    [SerializeField] private Sprite[] atkAnim;
    [SerializeField] private Sprite[] hurtAnim;
    [SerializeField] private Sprite[] dieAnim;
    private SpriteAnimatorCustom anim;

    [SerializeField] private Vector3 currentPos;
    [SerializeField] private Coroutine _currentRoutine;
    [SerializeField] private Coroutine _animationRoutine;
    private bool faceR = true;
    public bool GetFaceR()
    {
        return faceR;
    }

    [SerializeField]private int health;

    private BaseUnitDecoupleState decoupleState;
    private void Awake()
    {
        if (activeBaseUnitList == null)
        {
            activeBaseUnitList = new List<BaseUnit>();
        }
        activeBaseUnitList.Add(this);
    }

    private void Start()
    {
        health = 100;
        //currentPos = transform.position;
        anim = GetComponentInChildren<SpriteAnimatorCustom>();
        anim.SetFrameArray(idleAnim);
        activeAnimType = AnimationType.Idle;
        anim.PlayAnimationCustom(idleAnim, .1f);
        unitType = UnitType.Hobo;
        decoupleState = GetComponent<BaseUnitDecoupleState>();
        
    }

    private void Update()
    {
        Facing();
        if (!IsArrivedAtPosition())
        { 
            transform.position = Vector2.MoveTowards(transform.position, currentPos, 5 * Time.deltaTime);
            //PlayCharacterAnimation(AnimationType.Walk);
            //transform.position = Vector3.Lerp(transform.position, currentPos, Time.deltaTime * 5);
        }
        else if (IsArrivedAtPosition())
        {
            //PlayCharacterAnimation(AnimationType.Idle);
        }
    }

    public bool IsArrivedAtPosition()//for not go to transform
    {
        // Instead of setting a field directly return the value
        return Vector3.Distance(transform.position, currentPos) <= 0.0f;
    }

    public void SetUnitType(UnitType setUnitType)
    {
        unitType = setUnitType;
    }

    public void TakeDmg(int dmgAmt)
    {
        health -= dmgAmt;
        if(health <= 0)
        {
            OnKilled?.Invoke(this, EventArgs.Empty);
            //gameObject.SetActive(false);
        }
        else
        {
            OnTakeDamage?.Invoke(this, EventArgs.Empty);
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
    /*
    public void PlayIdleAnimation()
    {
        PlayCharacterAnimation(AnimationType.Idle);
    }
    public void PlayWalkAnimation()
    {
        PlayCharacterAnimation(AnimationType.Walk);
    }
    public void PlayAttackAnimation()
    {
        PlayCharacterAnimation(AnimationType.Attack);
    }
    public void PlayHurtAnimation()
    {
        PlayCharacterAnimation(AnimationType.Hurt);
    }
    public void PlayDieAnimation()
    {
        PlayCharacterAnimation(AnimationType.Die);
    }
    */

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
    public void Flip()
    {
        faceR = !faceR;
        /*
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
        */
        Vector3 spriteScaler = GetComponentInChildren<SpriteRenderer>().transform.localScale;
        spriteScaler.x *= -1;
        GetComponentInChildren<SpriteRenderer>().transform.localScale = spriteScaler;

    }

    
}
