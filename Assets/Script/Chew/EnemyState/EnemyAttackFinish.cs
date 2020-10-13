using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackFinish : IState<Enemy>
{
    private Vector3 direction;
    private float turnVelocity;

    // Start is called before the first frame update
    public void Enter(Enemy enemy)
    {
    }

    // Update is called once per frame
    public void Execute(Enemy enemy)
    {
        MoveAway(enemy);
    }

    public void Exit(Enemy enemy)
    { 
    }

    public void MoveAway(Enemy enemy)
    {
        Vector3 targetPos = enemy.TargetPlayer.transform.position;
        Vector3 tmpDir = (targetPos - enemy.transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z)*-1 * Mathf.Rad2Deg;
        direction = tmpDir;

        enemy.transform.position = new Vector3(enemy.transform.position.x + -direction.x * enemy.EnemyStat.movementSpeed * Time.deltaTime,
                     enemy.transform.position.y, enemy.transform.position.z + -direction.z * enemy.EnemyStat.movementSpeed * Time.deltaTime);

        float angle = Mathf.SmoothDampAngle(enemy.transform.eulerAngles.y, targetAngle, ref turnVelocity, 0.05f);
        enemy.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

        enemy.Anim.SetFloat("Speed", enemy.EnemyStat.movementSpeed);


        if (enemy.CheckDistance() >= (enemy.EnemyStat.visionRadius) /2)
        {
            enemy.ChangeState(new EnemyMovement());
        }
    }
}
