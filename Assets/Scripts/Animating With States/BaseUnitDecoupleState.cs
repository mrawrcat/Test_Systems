using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnitDecoupleState : MonoBehaviour
{
    public event EventHandler OnFoundEnemy;
    public event EventHandler OnEnemyNear;
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
    private SpriteAnimatorCustom anim;
    [Header("State Of Ally Unit")]
    [SerializeField] private State state;
    [SerializeField] private bool foundEnemy;
    [SerializeField] private bool enemyNear;
    public bool GetFoundEnemy()
    {
        return foundEnemy;
    }
    public bool GetEnemyNear()
    {
        return enemyNear;
    }
    [SerializeField] private Transform atkPos;
    [SerializeField] private Transform detectPos;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float detectSize;
    [SerializeField]private Vector2 detectBoxSize;
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
        OnEnemyNear += OnEnemyNear_Turn;
        baseUnit = GetComponent<BaseUnit>();
        anim = GetComponentInChildren<SpriteAnimatorCustom>();
        anim.OnAnimationFrameCounterIncrease += OnAnimationFrameCounterIncrease_DoStuff;
        anim.OnAnimationLoopedStopPlaying += OnAnimationLooped_StopPlaying;
        anim.OnAnimationLooped += OnAnimationLooped_Looped;
        anim.OnAnimationLoopedFirstTime += OnAnimationLooped_LoopedFirstTime;
        foundEnemy = false;
        enemyNear = false;
        nextAtkTime = 0;
        atkRate = 2f;
        baseUnit.OnTakeDamage += OnTakeDamage_PlayHurtAnimation;
        baseUnit.OnKilled += OnKilled_PlayDeathAnimation;
        whatIsEnemy = LayerMask.GetMask("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        foundEnemy = Physics2D.OverlapCircle(detectPos.position, detectSize, whatIsEnemy);
        enemyNear = Physics2D.OverlapBox(transform.position, detectBoxSize, whatIsEnemy);

        if (!enemyNear)
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
                    //state = State.Cower;
                }
            }
        }

        if (enemyNear)
        {
            if(state == State.Walk || state == State.Idle)
            {
                OnEnemyNear?.Invoke(this, EventArgs.Empty);
                if (baseUnit.unitType == BaseUnit.UnitType.Archer)
                {
                    state = State.AttackingMode;
                }
                else if (baseUnit.unitType == BaseUnit.UnitType.Hobo)
                {
                    //run or cower
                    //state = State.Cower;
                }
                else if(baseUnit.unitType == BaseUnit.UnitType.Villager)
                {
                    state = State.FearRun;
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

    private void OnTakeDamage_PlayHurtAnimation(object sender, EventArgs e)
    {
        state = State.Hurt;
        baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Hurt);
    }
    private void OnKilled_PlayDeathAnimation(object sender, EventArgs e)
    {
        state = State.Die;
        baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Die);
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
            //baseUnit.MoveTo(transform.position);
        }
        if(baseUnit.unitType == BaseUnit.UnitType.Villager)
        {
            //run if can else cower
            villagerAI.FinishTaskEarly();
            //baseUnit.MoveTo(transform.position);
        }
    }

    private void OnEnemyNear_Turn(object sender, EventArgs e)
    {
        if(baseUnit.unitType == BaseUnit.UnitType.Archer)
        {
            //stop moving, turn to face enemy, may need to fix later
            baseUnit.MoveTo(transform.position);
            BaseEnemy closestBaseEnemy = BaseEnemy.GetClosestEnemy(transform.position, detectSize);
            if (closestBaseEnemy != null)
            {
                Debug.Log("found enemy #" + closestBaseEnemy.GetIndexPositionInActiveBaseEnemyList());
                if(baseUnit.GetFaceR() == true && closestBaseEnemy.transform.position.x < transform.position.x)
                {
                    baseUnit.Flip();
                }
                else if(baseUnit.GetFaceR() == false && closestBaseEnemy.transform.position.x > transform.position.x)
                {
                    baseUnit.Flip();
                }
            }
        }
        if (baseUnit.unitType == BaseUnit.UnitType.Hobo)
        {
            //cower
            //baseUnit.MoveTo(transform.position);
        }
        if (baseUnit.unitType == BaseUnit.UnitType.Villager)
        {
            //run if can else cower
            //villagerAI.FinishTaskEarly();
            //baseUnit.MoveTo(transform.position);
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
        if (baseUnit.activeAnimType == BaseUnit.AnimationType.Hurt)
        {
            state = State.Idle;
            baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Idle);
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
        if (baseUnit.activeAnimType == BaseUnit.AnimationType.Hurt)
        {
            state = State.Idle;
            baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Idle);
        }
        if(baseUnit.activeAnimType == BaseUnit.AnimationType.Die)
        {
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectPos.position, detectSize);
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, detectBoxSize);
    }
}
