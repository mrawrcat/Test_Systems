using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private LevelWindow lvlWindow;
    private LevelSystem levelSystem = new LevelSystem();
    private void Awake()
    {
        lvlWindow.SetLevelSystem(levelSystem);
        LevelSystemAnimated levelSystemAnimated = new LevelSystemAnimated(levelSystem);
        lvlWindow.SetLevelSystemAnimated(levelSystemAnimated);
    }




    private void OnEnable()
    {
        //EnemyActions.OnEnemyHit += EnemyHit;
        //EnemyActions.OnEnemyDie += EnemyDie;
        //EnemyActions.OnIEnemyHit += IEnemyHit;
        //EnemyActions.OnIEnemyDie += IEnemyDie;

    }

    private void OnDisable()
    {
        //EnemyActions.OnEnemyHit -= EnemyHit;
        //EnemyActions.OnEnemyDie -= EnemyDie;
        //EnemyActions.OnIEnemyHit -= IEnemyHit;
        //EnemyActions.OnIEnemyDie -= IEnemyDie;
    }

    public void EnemyHit(Enemy enemy)
    {
        //GameManager.manager.score += 50f;
        //CamShake.camInstance.Shake(3, .1f); 
        //AudioManager.audioManager.Play("Hit");
        //enemy.TakeDmg(FindObjectOfType<PlayerStats>().GetBulletDamage());
        
    }

    public void EnemyDie(Enemy enemy)
    {
        //lvlWindow.AddExp(20);
        Debug.Log("Enemy Died");
    }

    public void IEnemyHit(TestInheritanceEnemy ienemy)
    {
        //CamShake.camInstance.Shake(3, .1f);
        //AudioManager.audioManager.Play("Hit");
        //ienemy.TakeDmg(FindObjectOfType<PlayerStats>().GetBulletDamage());
    }

    public void IEnemyDie(TestInheritanceEnemy enemy)
    {
        //lvlWindow.AddExp(20);
        Debug.Log("Enemy Died");
    }
    public void OnHit(EnemySpearman enemy)
    {
        //probably bounce backwards
        //hit animation, maybe blinking
    }

    public void OnDie(EnemySpearman enemy)
    {
        Debug.Log("Enemy Died");
    }


}
