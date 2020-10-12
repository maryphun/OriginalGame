using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : IState<Enemy>
{
    private float attDelay;
    private float tmpSpeed;
    private float attackedTime;
    // Start is called before the first frame update
    public void Enter(Enemy enemy)
    {
        attackedTime = Time.time;
        attDelay = 0;
        
    }

    // Update is called once per frame
    public void Execute(Enemy enemy)
    {
        if (enemy.CheckDistance() < enemy.EnemyStat.attackRange)
        {
            if (enemy.canAttack)
            {
                if (Time.time >= attackedTime + attDelay)
                {

                    enemy.canAttack = false;
                    enemy.Anim.Play("Attack", 0, 0.0f);
                    attackedTime = Time.time;
                    attDelay = enemy.EnemyStat.attackDelay;
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
