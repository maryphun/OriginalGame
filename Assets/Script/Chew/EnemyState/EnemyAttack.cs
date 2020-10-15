using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyAttack : IState<Enemy>
{
    private float attDelay;
    private float tmpSpeed;
    private float attackedTime;
    private bool hasAttacked;
    private float turnVelocity;
    // Start is called before the first frame update
    public void Enter(Enemy enemy)
    {
        attackedTime = Time.time;
        attDelay = 0;
        hasAttacked = false;


    }

    // Update is called once per frame
    public void Execute(Enemy enemy)
    {
        if (enemy.CheckDistance() < enemy.EnemyStat.attackRange + enemy.EnemyStat.attackRadiusOfArea / 2)
        {
            
            if (enemy.canAttack)
            {

                enemy.StartCoroutine(enemy.FaceDirection(enemy.TargetPlayer.transform.position));
                if (Time.time >= attackedTime + attDelay)
                {
                    if (!hasAttacked)
                    {
                        hasAttacked = true;
                        enemy.canAttack = false;
                        enemy.Anim.Play("Attack", 0, 0.0f);
                        attackedTime = Time.time;
                        attDelay = enemy.EnemyStat.attackDelay;
                    }
                    else
                    {
                        enemy.ChangeState(new EnemyAttackFinish());
                    }
                }
            }

        }
        else
        {
            if (!enemy.Anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                //Debug.Log("not attacking");
                enemy.ChangeState(new EnemyMovement());
            }
        }
    }

    public void Exit(Enemy enemy)
    {
    }


}
