using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyAttack : IState<Enemy>
{
    private bool hasAttacked;
    // Start is called before the first frame update
    public void Enter(Enemy enemy)
    {
        hasAttacked = false;

    }

    // Update is called once per frame
    public void Execute(Enemy enemy)
    {
        if (enemy.CheckPlayerDistance() < enemy.EnemyStat.attackRange + enemy.EnemyStat.attackRadiusOfArea / 2 
            || enemy.forceAttack)
        {
            if (enemy.canAttack)
            {
                enemy.FaceDirection(enemy.TargetPlayer.transform.position);
                if (!hasAttacked)
                {
                    enemy.StartCoroutine(enemy.AoeAttack());
                    //reset parameter value
                    hasAttacked = true;
                    enemy.canAttack = false;
                    enemy.Anim.Play("Attack", 0, 0.0f);
                    enemy.forceAttack = false;
                }
                else
                {
                    enemy.ChangeState(new EnemyAttackFinish());
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
