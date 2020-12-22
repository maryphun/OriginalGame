using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : IState<Enemy>
{
    const float hpBarFadeOutTime = 0.5f;
    private ItemDropEvent dropableItem;
    private EnemyHitPoint hpBar;
    private float deathTime;
    // Start is called before the first frame update
    public void Enter(Enemy enemy)
    {
        deathTime = Time.time;
        hpBar = enemy.GetComponent<EnemyHitPoint>();
        dropableItem = enemy.GetComponent<ItemDropEvent>();
        enemy.Anim.SetFloat("Speed", 0);
        enemy.Anim.Play("Death", 0, 0.0f);
    }

    // Update is called once per frame
    public void Execute(Enemy enemy)
    {
        enemy.isDying = true;
        if(enemy.isDying && Time.time >= (deathTime + hpBar.HpBarDisplayTime + hpBarFadeOutTime))
        {
           Exit(enemy);
        }
    }

    public void Exit(Enemy enemy)
    {
        WaveManager.enemyCount--;
        if (dropableItem)
        {
            dropableItem.DropItem();
        }
        SelfDestruct.Destroy(enemy.gameObject);

    }

}
