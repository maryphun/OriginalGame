using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovement : IState<Enemy>
{
    // Start is called before the first frame update
    public void Enter(Enemy enemy)
    {
        
    }

    // Update is called once per frame
    public void Execute(Enemy enemy)
    {
   
        enemy.agent.SetDestination(enemy.TargetPlayer.transform.position);
    }

    public void Exit(Enemy enemy)
    {

    }
}
