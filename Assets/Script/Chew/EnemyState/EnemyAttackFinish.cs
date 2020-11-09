using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyAttackFinish : IState<Enemy>
{
    private float timeNow;
    Vector3 direction;
    // Start is called before the first frame update
    public void Enter(Enemy enemy)
    {
        timeNow = Time.time;
        direction = (2 * enemy.transform.position - enemy.TargetPlayer.transform.position).normalized;
        enemy.Anim.SetFloat("Speed", enemy.EnemyStat.movementSpeed);
    }
        
    // Update is called once per frame
    public void Execute(Enemy enemy)
    {
        if (Time.time > timeNow + enemy.EnemyStat.attackDelay)
        {
            enemy.ChangeState(new EnemyMovement());
                
        }
        RaycastHit wallhit = new RaycastHit();
        if (enemy.CheckWallHit(1.0f,out wallhit))
        {
            direction = (Quaternion.AngleAxis(Random.Range(-70.0f, 70.0f), Vector3.up) * wallhit.normal);
            Debug.DrawRay(enemy.transform.position, direction,Color.blue,3f);

        }
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        enemy.transform.DORotateQuaternion(targetRotation,0.1f);
        enemy.transform.position = new Vector3(enemy.transform.position.x + direction.x * enemy.EnemyStat.movementSpeed * Time.deltaTime,
                             enemy.transform.position.y, enemy.transform.position.z + direction.z * enemy.EnemyStat.movementSpeed * Time.deltaTime);
    }

    public void Exit(Enemy enemy)
    {
        enemy.Anim.SetFloat("Speed", 0);

    }
}
