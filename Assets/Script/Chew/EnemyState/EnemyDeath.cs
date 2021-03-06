﻿using System.Collections;
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
        if (dropableItem)
        {
            dropableItem.DropItem();
        }
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
        SelfDestruct.Destroy(enemy.gameObject);

    }

}
