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
        Vector3 tmpDir = (targetPos - enemy.transform.position).normalized;
        RaycastHit raycastHit;

        float offset = enemy.EnemyStat.attackRange;

        if (enemy.CheckPlayerDistance() < enemy.EnemyStat.escapeRange)
        {
            enemy.FaceDirection(enemy.TargetPlayer.transform.position,true);

            if (enemy.CheckWallHit(enemy.GetWallHitDistance,out raycastHit) || enemy.CheckWallHit(enemy.GetWallHitDistance, out raycastHit,true))
            {
                enemy.forceAttack = true;
                enemy.ChangeState(new EnemyAttack());
            }
            else
            {
                enemy.ChangeState(new EnemyEscape());
            }

        }
        else if (enemy.CheckPlayerDistance() < (offset + enemy.EnemyStat.attackRadiusOfArea / 2))
        {
            enemy.ChangeState(new EnemyAttack());
        }
        else
        {
                direction = tmpDir;
                enemy.FaceDirection(enemy.TargetPlayer.transform.position);
                enemy.transform.position = new Vector3(enemy.transform.position.x + direction.x * enemy.EnemyStat.movementSpeed * Time.deltaTime,
                             enemy.transform.position.y, enemy.transform.position.z + direction.z * enemy.EnemyStat.movementSpeed * Time.deltaTime);

                enemy.Anim.SetFloat("Speed", enemy.EnemyStat.movementSpeed);
        }
    }

    public void Exit(Enemy enemy)
    {
        enemy.Anim.SetFloat("Speed", 0);

    }


}
