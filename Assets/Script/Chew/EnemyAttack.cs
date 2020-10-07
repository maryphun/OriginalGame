using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : IState<Enemy>
{
    // Start is called before the first frame update
    public void Enter(Enemy enemy)
    {
        
    }

    // Update is called once per frame
    public void Execute(Enemy enemy)
    {
        if (enemy.CheckDistance() > enemy.EnemyStat.attackRange)
        {
            enemy.ChangeState(new EnemyMovement());
        }
    }

    public void Exit(Enemy enemy)
    {

    }
}
