using System;

public static class EnemyActions
{
   // public static Action<Enemy> OnEnemyHit;
    //public static Action<Enemy> OnEnemyDie;
    public static Action<TestInheritanceEnemy> OnIEnemyHit;
    public static Action<TestInheritanceEnemy> OnIEnemyDie;
}
