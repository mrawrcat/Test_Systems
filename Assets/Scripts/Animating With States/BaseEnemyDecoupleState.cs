using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyDecoupleState : MonoBehaviour
{
    public event EventHandler OnFoundEnemy;
    private enum State
    {
        Idle,
        Walk,
        Attack,
        AttackingMode,
        Hurt,
        Die,
    }
    [SerializeField] private State state;
    private SpriteAnimatorCustom anim;
    [SerializeField] private bool foundEnemy;
    [SerializeField] private Transform atkPos;
    [SerializeField] private Transform detectPos;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float detectSize;
    [SerializeField] private float atkRate;
    private float nextAtkTime;
    private BaseEnemy baseEnemyUnit;
    [SerializeField]private List<BaseUnit> detectedEnemyList;
    //private TaskTestNewWorkerAI ttworkerAI; //need to find the tasksystem of this unit
    private Vector3 enemyTargetPos;
    private float calculateDist;
    private float storedSmallestDist = 20f;
    // Start is called before the first frame update
    void Start()
    {
        detectedEnemyList = new List<BaseUnit>();
        //ttworkerAI = GetComponent<TaskTestNewWorkerAI>();
        OnFoundEnemy += OnFoundEnemy_EnemyFound;
        baseEnemyUnit = GetComponent<BaseEnemy>();
        anim = GetComponentInChildren<SpriteAnimatorCustom>();
        anim.OnAnimationLoopedStopPlaying += OnAnimationLooped_StopPlaying;
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
            if (!baseEnemyUnit.IsArrivedAtPosition())
            {
                state = State.Walk;
                baseEnemyUnit.PlayCharacterAnimation(BaseEnemy.AnimationType.Walk);
            }
            else if(baseEnemyUnit.IsArrivedAtPosition())
            {
                state = State.Idle;
                baseEnemyUnit.PlayCharacterAnimation(BaseEnemy.AnimationType.Idle);
            }
        }

        if (foundEnemy)
        {
            //Debug.Log("found the enemy");
            if (state == State.Walk || state == State.Idle)
            {
                OnFoundEnemy?.Invoke(this, EventArgs.Empty);
                state = State.AttackingMode;


            }
        }

        if (state == State.AttackingMode)
        {
            if (Time.time > nextAtkTime)
            {
                //TryDoAttack();
                testdoAtk();
                Debug.Log("Try do Attack");
                nextAtkTime = Time.time + atkRate;
            }
            /*
            if (foundEnemy)
            {
            }
            else
            {
                state = State.Idle;
                baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Idle);
            }*/
        }
    }

    private void OnFoundEnemy_EnemyFound(object sender, EventArgs e)
    {
        Debug.Log("try to stop moving as soon as found enemy");
        //ttworkerAI.FinishTaskEarly();
        baseEnemyUnit.MoveTo(transform.position, () => { Debug.Log("enemy found ally, execute stop moving (not task)"); });
    }

    private void testdoAtk()
    {
        state = State.Attack;
        if(state == State.Attack)
        {
            //calculate which enemy to target -> shoot arrow
            Collider2D[] detectedEnemies = Physics2D.OverlapCircleAll(detectPos.position, detectSize, whatIsEnemy);
            foreach (Collider2D enemy in detectedEnemies)
            {
                BaseUnit baseUnit = enemy.GetComponent<BaseUnit>();
                if (baseUnit != null)
                {
                    detectedEnemyList.Add(baseUnit);
                }
            }
            if(detectedEnemyList.Count > 0)
            {
                //set takedmg of ally
                detectedEnemyList[0].GetComponent<BaseUnit>().TakeDmg(10);
            }
            detectedEnemyList.Clear();
                
        }
        baseEnemyUnit.PlayCharacterAnimation(BaseEnemy.AnimationType.Attack);
    }
    


    private void OnAnimationLooped_StopPlaying(object sender, EventArgs e)
    {
        if(baseEnemyUnit.activeAnimType == BaseEnemy.AnimationType.Attack)//dont think will attack without enemy found but this code still works
        {
            if (foundEnemy)
            {
                state = State.AttackingMode;
                baseEnemyUnit.PlayCharacterAnimation(BaseEnemy.AnimationType.Idle);
            }
            else
            {
                state = State.Idle;
                baseEnemyUnit.PlayCharacterAnimation(BaseEnemy.AnimationType.Idle);
            }
        } 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectPos.position, detectSize);
    }
}
