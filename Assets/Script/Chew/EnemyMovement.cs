﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovement : IState<Enemy>
{
    private float turnVelocity;
    private Vector3 direction;
    // Start is called before the first frame update
    public void Enter(Enemy enemy)
    {
        direction = (enemy.TargetPlayer.transform.position - enemy.transform.position).normalized;
    }
    

    // Update is called once per frame
    public void Execute(Enemy enemy)
    {
        LayerMask wallMask = LayerMask.GetMask("Wall");
        LayerMask playerMask = LayerMask.GetMask("Player");
        RaycastHit wallRayHit = new RaycastHit();
        Ray forwardRay = new Ray(enemy.transform.position, enemy.transform.forward);
        var tmpRandomizeAngle = Random.Range(-enemy.EnemyStat.visionAngle / 4, enemy.EnemyStat.visionAngle / 4);
        Ray leftRay = new Ray(enemy.transform.position, Quaternion.AngleAxis(-enemy.EnemyStat.visionAngle / 2 + tmpRandomizeAngle, Vector3.up) * enemy.transform.forward);
        Ray rightRay = new Ray(enemy.transform.position, Quaternion.AngleAxis(enemy.EnemyStat.visionAngle / 2 + tmpRandomizeAngle, Vector3.up) * enemy.transform.forward);

        if (Physics.Raycast(forwardRay,out wallRayHit, enemy.EnemyStat.visionRadius,wallMask))
        {
         
            if (!Physics.Raycast(leftRay, enemy.EnemyStat.visionRadius, wallMask))
            {
                direction = leftRay.direction;
            }
            else if (!Physics.Raycast(rightRay, enemy.EnemyStat.visionRadius, wallMask))
            {
                direction = rightRay.direction;
            }
            else
            {
                RaycastHit playerRayHit = new RaycastHit();
                if (Physics.Raycast(forwardRay, out playerRayHit, enemy.EnemyStat.visionRadius, playerMask))
                {
                    Debug.DrawLine(enemy.transform.position, enemy.transform.forward * enemy.EnemyStat.visionRadius, Color.red);

                    if (playerRayHit.distance < wallRayHit.distance)
                    {
                        direction = forwardRay.direction;
                    }
                }
                else
                {
                    direction = leftRay.direction;
                }
            }
            Debug.DrawLine(enemy.transform.position, enemy.transform.forward * enemy.EnemyStat.visionRadius, Color.red);
        }
        else
        {
            Vector3 tmpDir = (enemy.TargetPlayer.transform.position - enemy.transform.position).normalized;
            if (!Physics.Raycast(enemy.transform.position, tmpDir, enemy.EnemyStat.visionRadius, wallMask))
            {
                direction = tmpDir;

            }
            Debug.DrawLine(enemy.transform.position, enemy.transform.forward * enemy.EnemyStat.visionRadius, Color.green);
        }

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        enemy.transform.position = new Vector3(enemy.transform.position.x + direction.x * enemy.EnemyStat.movementSpeed * Time.deltaTime,
                        enemy.transform.position.y, enemy.transform.position.z + direction.z * enemy.EnemyStat.movementSpeed * Time.deltaTime);

        float angle = Mathf.SmoothDampAngle(enemy.transform.eulerAngles.y, targetAngle,ref turnVelocity, 0.05f);
        enemy.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    public void Exit(Enemy enemy)
    {

    }
}
