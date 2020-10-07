using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGetHit : IState<Enemy>
{
    private float timeNow;
    // Start is called before the first frame update
    public void Enter(Enemy enemy)
    {
        timeNow = Time.time;
    }

    public void Execute(Enemy enemy)
    {
        if (timeNow <= Time.time - enemy.EnemyStat.stunTime)
        {
            enemy.ChangeState(new EnemyMovement());
        }
    }

    public void Exit(Enemy enemy)
    {
    }
   }
