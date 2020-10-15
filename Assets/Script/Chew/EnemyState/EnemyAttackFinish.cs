using System.Collections;
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
        MoveAway(enemy);
        if (enemy.CheckDistance() >= (enemy.EnemyStat.visionRadius) || Time.time > timeNow + actionTime)
        {
            enemy.ChangeState(new EnemyMovement());
        }
    }

    public void Exit(Enemy enemy)
    {
        enemy.Anim.SetFloat("Speed", 0);

    }

    public void MoveAway(Enemy enemy)
    {
        Vector3 targetPos = enemy.TargetPlayer.transform.position;
        Vector3 tmpDir = (targetPos - enemy.transform.position).normalized;
        direction = tmpDir;
        float targetAngle = (Mathf.Atan2(-direction.x, -direction.z) * Mathf.Rad2Deg);
        enemy.transform.position = new Vector3(enemy.transform.position.x + -direction.x * enemy.EnemyStat.movementSpeed * Time.deltaTime,
                     enemy.transform.position.y, enemy.transform.position.z + -direction.z * enemy.EnemyStat.movementSpeed * Time.deltaTime);

        float angle = Mathf.SmoothDampAngle(enemy.transform.eulerAngles.y, targetAngle, ref turnVelocity, 0.05f);
        enemy.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

        enemy.Anim.SetFloat("Speed", enemy.EnemyStat.movementSpeed);

        
    }
}
