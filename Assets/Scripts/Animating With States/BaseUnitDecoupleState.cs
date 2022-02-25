using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnitDecoupleState : MonoBehaviour
{
    public event EventHandler OnFoundEnemy;

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
    [SerializeField] private bool foundEnemy;
    public bool GetFoundEnemy()
    {
        return foundEnemy;
    }
    [SerializeField] private Transform atkPos;
    [SerializeField] private Transform detectPos;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float detectSize;
    [SerializeField] private float atkRate;
    private float nextAtkTime;
    private BaseUnit baseUnit;
    [SerializeField]private List<BaseEnemy> detectedEnemyList;
    private TaskTestHoboAI hoboAI;
    private TaskTestVillagerAI villagerAI;
    private TaskTestNewWorkerAI ttworkerAI; //needs all of the worker AI
   
    // Start is called before the first frame update
    void Start()
    {
        detectedEnemyList = new List<BaseEnemy>();
        hoboAI = GetComponent<TaskTestHoboAI>();
        villagerAI = GetComponent<TaskTestVillagerAI>();
        ttworkerAI = GetComponent<TaskTestNewWorkerAI>();
        OnFoundEnemy += OnFoundEnemy_EnemyFound;
        baseUnit = GetComponent<BaseUnit>();
        anim = GetComponentInChildren<SpriteAnimatorCustom>();
        anim.OnAnimationFrameCounterIncrease += OnAnimationFrameCounterIncrease_DoStuff;
        anim.OnAnimationLoopedStopPlaying += OnAnimationLooped_StopPlaying;
        anim.OnAnimationLooped += OnAnimationLooped_Looped;
        anim.OnAnimationLoopedFirstTime += OnAnimationLooped_LoopedFirstTime;
        foundEnemy = false;
        nextAtkTime = 0;
        atkRate = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        foundEnemy = Physics2D.OverlapCircle(detectPos.position, detectSize, whatIsEnemy);
        
        if (!foundEnemy)
        {
            if(baseUnit.unitType == BaseUnit.UnitType.Hobo)
            {
                //Roam();
            }
            if (!baseUnit.IsArrivedAtPosition())
            {
                state = State.Walk;
                baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Walk);
            }
            

        }
        if (foundEnemy)
        {
            //Debug.Log("found the enemy");
            if (state == State.Walk || state == State.Idle)
            {
                OnFoundEnemy?.Invoke(this, EventArgs.Empty);
                if(baseUnit.unitType == BaseUnit.UnitType.Archer)
                {
                    state = State.AttackingMode;
                }
                else if(baseUnit.unitType == BaseUnit.UnitType.Hobo)
                {
                    //run or cower
                    state = State.Cower;
                }
            }
            


        }

        if (state == State.AttackingMode)
        {
            if (Time.time > nextAtkTime)
            {
                //TryDoAttack();
                testdoAtk();
                nextAtkTime = Time.time + atkRate;
            }
        }
    }

    

    private void OnFoundEnemy_EnemyFound(object sender, EventArgs e)
    {
        Debug.Log("try to stop moving as soon as found enemy");
        //need to save task here
        if(baseUnit.unitType == BaseUnit.UnitType.Archer)
        {
            ttworkerAI.FinishTaskEarly();//for now worker is archer need to make new ai for archer and builder
            baseUnit.MoveTo(transform.position);
        }
        if(baseUnit.unitType == BaseUnit.UnitType.Hobo)
        {
            //cower
            baseUnit.MoveTo(transform.position);
        }
        if(baseUnit.unitType == BaseUnit.UnitType.Villager)
        {
            //run if can else cower
            villagerAI.FinishTaskEarly();
            baseUnit.MoveTo(transform.position);
        }
    }

    private void testdoAtk()//might need a whole seperate attack system later
    {
        state = State.Attack;
        if(state == State.Attack)
        {
            BaseEnemy closestBaseEnemy = BaseEnemy.GetClosestEnemy(transform.position, detectSize);//maybe detectSize might not be right but still works for now + no way of visualizing
            if (closestBaseEnemy != null)
            {
                Debug.Log("found enemy #" + closestBaseEnemy.GetIndexPositionInActiveBaseEnemyList());
                Arrow.Create_Arrow(atkPos.position, closestBaseEnemy.transform.position, 25f);
            }
            baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Attack);
        }
    }

    private void OnAnimationFrameCounterIncrease_DoStuff(object sender, EventArgs e)
    {
        if (!foundEnemy)
        {
            if (!baseUnit.IsArrivedAtPosition())
            {
                state = State.Walk;
                baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Walk);
            }
            else if (baseUnit.IsArrivedAtPosition())
            {
                state = State.Idle;
                baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Idle);
            }
        }
    }

    private void OnAnimationLooped_LoopedFirstTime(object sender, EventArgs e)
    {
        if (!foundEnemy)
        {
            if (baseUnit.GetComponent<TaskTestNewWorkerAI>().GetSavedTestTask() != null)
            {
                baseUnit.GetComponent<TaskTestNewWorkerAI>().DoSavedTask();
            }

            if (!baseUnit.IsArrivedAtPosition())
            {
                state = State.Walk;
                baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Walk);
            }
            else if (baseUnit.IsArrivedAtPosition())
            {
                state = State.Idle;
                baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Idle);
            }
        }
    }

    private void OnAnimationLooped_Looped(object sender, EventArgs e)
    {
        if (!foundEnemy)
        {

            if(baseUnit.GetComponent<TaskTestNewWorkerAI>().GetSavedTestTask() != null)
            {
                baseUnit.GetComponent<TaskTestNewWorkerAI>().DoSavedTask();

            }

            if (!baseUnit.IsArrivedAtPosition())
            {
                state = State.Walk;
                baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Walk);
            }
            else if (baseUnit.IsArrivedAtPosition())
            {
                state = State.Idle;
                baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Idle);
            }
        }
    }

    private void OnAnimationLooped_StopPlaying(object sender, EventArgs e)
    {
        //basically without enemy being found attack animation never starts playing
        if(baseUnit.activeAnimType == BaseUnit.AnimationType.Attack)
        {
            if (foundEnemy)
            {
                state = State.AttackingMode;
                baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Idle);
                Debug.Log("Shot arrow at an enemy go back to idle pos");
            }
            else //i think this usually never gets called because foundEnemy is always true because arrow didnt kill it before animation stopped
            {
                if (baseUnit.GetComponent<TaskTestNewWorkerAI>().GetSavedTestTask() != null)
                {
                    baseUnit.GetComponent<TaskTestNewWorkerAI>().DoSavedTask();
                }
                else
                {
                    state = State.Idle;
                    baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Idle);
                    Debug.Log("Killed all enemies and no saved task, go back to idle pos");

                }
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectPos.position, detectSize);
    }
}
