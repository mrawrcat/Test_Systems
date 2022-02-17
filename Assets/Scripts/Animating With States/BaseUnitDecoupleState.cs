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
    private TaskGameHandler taskGameHandler;
    [SerializeField] private float atkRate;
    private float nextAtkTime;
    private BaseUnit baseUnit;
    [SerializeField]private List<BaseEnemy> detectedEnemyList;
    private TaskTestNewWorkerAI ttworkerAI;
    // Start is called before the first frame update
    void Start()
    {
        detectedEnemyList = new List<BaseEnemy>();
        ttworkerAI = GetComponent<TaskTestNewWorkerAI>();
        OnFoundEnemy += OnFoundEnemy_EnemyFound;
        baseUnit = GetComponent<BaseUnit>();
        anim = GetComponentInChildren<SpriteAnimatorCustom>();
        anim.OnAnimationLoopedStopPlaying += OnAnimationLooped_StopPlaying;
        foundEnemy = false;
        taskGameHandler = FindObjectOfType<TaskGameHandler>();
        nextAtkTime = 0;
        atkRate = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        foundEnemy = Physics2D.OverlapCircle(detectPos.position, detectSize, whatIsEnemy);
        
        if (!foundEnemy)
        {
            if (!baseUnit.IsArrivedAtPosition())
            {
                state = State.Walk;
                baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Walk);
            }
            else if(baseUnit.IsArrivedAtPosition())
            {
                state = State.Idle;
                baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Idle);
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
        ttworkerAI.FinishTaskEarly();
        baseUnit.MoveTo(transform.position);
    }

    private void testdoAtk()
    {
        state = State.Attack;
        if(state == State.Attack)
        {
            /*
            //calculate which enemy to target -> shoot arrow
            Collider2D[] detectedEnemies = Physics2D.OverlapCircleAll(detectPos.position, detectSize, whatIsEnemy);
            foreach (Collider2D enemy in detectedEnemies)
            {
                BaseEnemy baseEnemy = enemy.GetComponent<BaseEnemy>();
                if (baseEnemy != null)
                {
                    detectedEnemyList.Add(baseEnemy);
                }
            }
            if(detectedEnemyList.Count > 0)
            {
                for(int i = 0; i < detectedEnemyList.Count; i++)
                {
                    calculateDist = Vector3.Distance(transform.position, detectedEnemyList[i].transform.position);
                    Debug.Log("calculated distance: " + calculateDist);
                    if(calculateDist < storedSmallestDist)
                    {
                        storedSmallestDist = calculateDist;
                        enemyTargetPos = detectedEnemyList[i].transform.position;
                    }
                    //calculate distance from enemy, if this one's calculateDist is smaller than the previous one this one's calculateDist is the smallest one
                    Debug.Log("enemy target pos: " + enemyTargetPos);
                }
            }
            detectedEnemyList.Clear();
            */
            //float hitDetection = 5f;
            BaseEnemy closestBaseEnemy = BaseEnemy.GetClosestEnemy(transform.position, detectSize);
            if (closestBaseEnemy != null)
            {
                Debug.Log("found enemy #" + closestBaseEnemy.GetIndexPositionInActiveBaseEnemyList());
                Arrow.Create_Arrow(atkPos.position, closestBaseEnemy.transform.position, 25f);
            }
            baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Attack);
        }
    }

    private void TryDoAttack()
    {
        state = State.Attack;
        if (state == State.Attack)
        {

            TaskGameHandler.TestTask testTask = new TaskGameHandler.TestTask.StopAndAttack
            {
                AttackAction = (TaskTestNewWorkerAI) =>
                {
                    Debug.Log("try to attack enemy");
                    //PlayCharacterAnimation(AnimationType.Attack);
                }
            };
            taskGameHandler.testTaskSystem.AddTask(testTask);

            Debug.Log("Try to do attack");
        }


    }

    private void OnAnimationLooped_StopPlaying(object sender, EventArgs e)
    {
        if(baseUnit.activeAnimType == BaseUnit.AnimationType.Attack)//dont think will attack without enemy found but this code still works
        {
            if (foundEnemy)
            {
                state = State.AttackingMode;
                baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Idle);
            }
            else
            {
                state = State.Idle;
                baseUnit.PlayCharacterAnimation(BaseUnit.AnimationType.Idle);
            }
        } 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectPos.position, detectSize);
    }
}
