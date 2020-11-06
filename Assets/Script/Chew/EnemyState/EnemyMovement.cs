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
        Vector3 targetPos = enemy.TargetPlayer.transform.position;
        //LayerMask wallMask = LayerMask.GetMask("Wall");
        //LayerMask playerMask = LayerMask.GetMask("Player");
        //RaycastHit wallRayHit = new RaycastHit();
        //Ray forwardRay = new Ray(enemy.transform.position + Vector3.up, enemy.transform.forward);
        //Ray leftRay = new Ray(enemy.transform.position + Vector3.up, Quaternion.AngleAxis(-enemy.EnemyStat.visionAngle , Vector3.up) * enemy.transform.forward);
        //Ray rightRay = new Ray(enemy.transform.position + Vector3.up, Quaternion.AngleAxis(enemy.EnemyStat.visionAngle, Vector3.up) * enemy.transform.forward);
        Vector3 tmpDir = (targetPos - enemy.transform.position).normalized;


        // Debug.Log("pos" + forwardRay.origin.y);
        //if (Physics.Raycast(forwardRay,out wallRayHit, enemy.EnemyStat.visionRadius,wallMask))
        //{

        //    if (!Physics.Raycast(leftRay, enemy.EnemyStat.visionRadius, wallMask))
        //    {
        //        direction = leftRay.direction;
        //    }
        //    else if (!Physics.Raycast(rightRay, enemy.EnemyStat.visionRadius, wallMask))
        //    {
        //        direction = rightRay.direction;
        //    }
        //    else
        //    {
        //RaycastHit playerRayHit = new RaycastHit();
        //        if (DetectObject(enemy.transform, enemy.TargetPlayer.transform, enemy.EnemyStat.visionAngle / 2, enemy.EnemyStat.visionRadius))
        //        {
        //            Debug.DrawLine(enemy.transform.position, enemy.transform.forward * enemy.EnemyStat.visionRadius, Color.red);

        //    //if (playerRayHit.distance < wallRayHit.distance)
        //    //{

        //            //}
        //        }
        //        else
        //{
        //    direction =enemy.transform.forward;
        //}
        //if (Vector3.Distance(targetPos,enemy.transform.position) > Vector3.Distance(targetPos + enemy.TargetPlayer.transform.forward, enemy.transform.position))
        //{
        //    direction = leftRay.direction;
        //}
        //        else
        //        {
        //            direction = leftRay.direction;
        //        }
        //    }
        //    Debug.DrawLine(enemy.transform.position, enemy.transform.forward * enemy.EnemyStat.visionRadius, Color.red);
        //}
        //else
        //{
        //    Vector3 tmpDir = (enemy.TargetPlayer.transform.position - enemy.transform.position).normalized;
        //    if (!Physics.Raycast(enemy.transform.position, tmpDir, enemy.EnemyStat.visionRadius, wallMask))
        //    {
        //        direction = tmpDir;

        //    }
        //    Debug.DrawLine(enemy.transform.position, enemy.transform.forward * enemy.EnemyStat.visionRadius, Color.green);
        //}
        float offset = enemy.EnemyStat.attackRange;

        if (enemy.CheckPlayerDistance() < enemy.EnemyStat.escapeRange)
        {
            enemy.FaceDirection(enemy.TargetPlayer.transform.position,true);

            if (enemy.CheckWallHit(enemy.GetWallHitDistance))
            {
                enemy.forceAttack = true;
                enemy.ChangeState(new EnemyAttack());
            }
            else
            {
               enemy.MoveAwayFromPlayer();
            }

        }
        else if (enemy.CheckPlayerDistance() < (offset + enemy.EnemyStat.attackRadiusOfArea / 2))
        {
            enemy.ChangeState(new EnemyAttack());
        }
        else
        {
            if (enemy.CheckPlayerDistance() <  enemy.EnemyStat.visionRadius)
            {
                direction = tmpDir;
                enemy.FaceDirection(enemy.TargetPlayer.transform.position);
                enemy.transform.position = new Vector3(enemy.transform.position.x + direction.x * enemy.EnemyStat.movementSpeed * Time.deltaTime,
                             enemy.transform.position.y, enemy.transform.position.z + direction.z * enemy.EnemyStat.movementSpeed * Time.deltaTime);

                enemy.Anim.SetFloat("Speed", enemy.EnemyStat.movementSpeed);
            }
        }
      

    }

    public void Exit(Enemy enemy)
    {
        enemy.Anim.SetFloat("Speed", 0);

    }


}
