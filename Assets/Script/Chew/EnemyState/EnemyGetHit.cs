using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGetHit : IState<Enemy>
{
    private float timeNow;
    // Start is called before the first frame update
    public void Enter(Enemy enemy)
    {
        enemy.Anim.Play("Stun", 0, 0.0f);
        enemy.Anim.SetFloat("Speed", 0);
        timeNow = Time.time;
    }

    public void Execute(Enemy enemy)
    {
        //if (enemy.canMove)
        //{
        //    enemy.canMove = false;
        //}

        if (Time.time > timeNow + enemy.EnemyStat.stunTime)
        {
            enemy.ChangeState(new EnemyMovement());
        }
    }

    public void Exit(Enemy enemy)
    {
    }
   }
