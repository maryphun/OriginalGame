﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackFinish : IState<Enemy>
{
    private Vector3 direction;
    private float turnVelocity;
    private float actionTime;
    private float timeNow;
    // Start is called before the first frame update
    public void Enter(Enemy enemy)
    {
        timeNow = Time.time;
        actionTime = 1.5f;

    }
        
    // Update is called once per frame
    public void Execute(Enemy enemy)
    {
        if (!enemy.CheckWallHit(enemy.GetWallHitDistance, true))
        {
            enemy.StartCoroutine(enemy.FaceDirection(2 * enemy.transform.position - enemy.TargetPlayer.transform.position));
            enemy.MoveAwayFromPlayer();
        }
        if (enemy.CheckPlayerDistance() >= (enemy.EnemyStat.visionRadius) || Time.time > timeNow + actionTime)
        {
            enemy.ChangeState(new EnemyMovement());
        }
    }

    public void Exit(Enemy enemy)
    {
        enemy.Anim.SetFloat("Speed", 0);

    }
}
