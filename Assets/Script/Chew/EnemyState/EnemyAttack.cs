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
                    enemy.StartCoroutine(AoeAttack(enemy,enemy.EnemyStat.followTarget));
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

    public IEnumerator AoeAttack(Enemy enemy)
    {
        if (enemy.EnemyStat.attackType != AttackType.AreaMelee && enemy.EnemyStat.attackType != AttackType.AreaRanged)
        {
            yield break;
        }
        if (enemy.CheckPlayerDistance() < enemy.EnemyStat.attackRange)
        {
            enemy.aoeAimPoint = enemy.transform.position + ((enemy.TargetPlayer.transform.position - enemy.transform.position).normalized * enemy.CheckPlayerDistance());
        }
        else
        {
            enemy.aoeAimPoint = enemy.transform.position + ((enemy.TargetPlayer.transform.position - enemy.transform.position).normalized * enemy.EnemyStat.attackRange);
        }
        enemy.aoeAimPoint.y = 0.1f;
        if (enemy.EnemyStat.attackType == AttackType.AreaRanged)
        {
            enemy.projectileMng.InitiateProjectile(enemy.transform, enemy.EnemyStat.projectiles.transform, enemy.transform.position, enemy.aoeAimPoint, enemy.EnemyStat.indicatorTime, null);
        }

        var aoe = GameObject.Instantiate(enemy.EnemyStat.aoeIndicator, enemy.aoeAimPoint, enemy.EnemyStat.aoeIndicator.transform.rotation) as GameObject;
        //radius of effect is set to 0.5f as default 
        aoe.transform.localScale *= (enemy.EnemyStat.attackRadiusOfArea * 2);
        //wait the aoe circle to reach its maximum size before the effect occur
        GameObject.Destroy(aoe, enemy.EnemyStat.indicatorTime);
        var aoeEffect = GameObject.Instantiate(enemy.EnemyStat.indicatorEffect, enemy.aoeAimPoint, enemy.EnemyStat.indicatorEffect.transform.rotation) as GameObject;
        aoeEffect.transform.localScale *= (enemy.EnemyStat.attackRadiusOfArea * 2);
        GameObject.Destroy(aoeEffect, enemy.EnemyStat.indicatorTime);
        yield return new WaitForSeconds(enemy.EnemyStat.indicatorTime);
        enemy.DealDamage();
    }

    public IEnumerator AoeAttack(Enemy enemy,bool followTarget, float followTime = 0.5f)
    {
        if (!followTarget)
        {
            enemy.StartCoroutine(AoeAttack(enemy));
            yield break;
        }
        if (enemy.EnemyStat.attackType != AttackType.AreaMelee && enemy.EnemyStat.attackType != AttackType.AreaRanged)
        {
            yield break;
        }
        enemy.aoeAimPoint = enemy.TargetPlayer.transform.position;
        enemy.aoeAimPoint.y = 0.1f;

        if (enemy.EnemyStat.attackType == AttackType.AreaRanged)
        {
            enemy.projectileMng.InitiateProjectile(enemy.transform, enemy.EnemyStat.projectiles.transform, enemy.transform.position, enemy.TargetPlayer.transform, followTime, enemy.EnemyStat.indicatorTime, null);
            Debug.Log("Follow Target");
        }
        float stopFollowTime = (enemy.EnemyStat.indicatorTime) * followTime;
        float time = 0;
        var aoe = GameObject.Instantiate(enemy.EnemyStat.aoeIndicator, enemy.TargetPlayer.transform.position, enemy.EnemyStat.aoeIndicator.transform.rotation) as GameObject;
        //radius of effect is set to 0.5f as default 
        aoe.transform.localScale *= (enemy.EnemyStat.attackRadiusOfArea * 2);
        GameObject.Destroy(aoe, (enemy.EnemyStat.indicatorTime));

        while (time < stopFollowTime)
        {
            aoe.transform.position = enemy.TargetPlayer.transform.position;
            time += Time.deltaTime;

            yield return null;
        }
        //wait the aoe circle to stop before the effect occur
        Debug.Log(enemy.TargetPlayer.transform.position);
        var aoeEffect = GameObject.Instantiate(enemy.EnemyStat.indicatorEffect, aoe.transform.position, enemy.EnemyStat.indicatorEffect.transform.rotation) as GameObject;
        aoeEffect.transform.localScale *= (enemy.EnemyStat.attackRadiusOfArea * 2);
        GameObject.Destroy(aoeEffect, enemy.EnemyStat.indicatorTime - stopFollowTime);
        yield return new WaitForSeconds(enemy.EnemyStat.indicatorTime - stopFollowTime);
        enemy.DealDamage();
    }
}
