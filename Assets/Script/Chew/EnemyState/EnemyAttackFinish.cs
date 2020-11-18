using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyAttackFinish : IState<Enemy>
{
    private float timeNow;
    // Start is called before the first frame update
    public void Enter(Enemy enemy)
    {
        timeNow = Time.time;
    }
        
    // Update is called once per frame
    public void Execute(Enemy enemy)
    {
        if (Time.time > timeNow + enemy.EnemyStat.attackDelay)
        {
            enemy.ChangeState(new EnemyMovement());
        }
        if (enemy.EnemyStat.escapeAfterAttack)
        {
            enemy.ChangeState(new EnemyEscape(enemy.EnemyStat.attackDelay));
        }
    }

    public void Exit(Enemy enemy)
    {
        enemy.Anim.SetFloat("Speed", 0);
    }
}
