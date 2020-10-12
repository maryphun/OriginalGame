using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : IState<Enemy>
{
    private ItemDropEvent dropableItem;
    // Start is called before the first frame update
    public void Enter(Enemy enemy)
    {
        dropableItem = enemy.GetComponent<ItemDropEvent>();
        if (dropableItem)
        {
            dropableItem.DropItem();
        }
        SelfDestruct.Destroy(enemy.gameObject);
    }

    // Update is called once per frame
    public void Execute(Enemy enemy)
    {
        
    }

    public void Exit(Enemy enemy)
    { 
    }

}
