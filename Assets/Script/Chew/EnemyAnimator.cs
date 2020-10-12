﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Enemy enemyScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetCanAttack(int boolean)
    {
        enemyScript.canAttack = (boolean != 0);

        if (enemyScript.canAttack)
        {
            //enemyScript.ResetMoveSpeedMax();
            //enemyScript.SetIsAttacking(false);
        }
    }

    public void SetCanMove(int boolean)
    {
        enemyScript.canMove = (boolean != 0);

        if (enemyScript.canMove)
        {
            //enemyScript.ResetMoveSpeedMax();
            //enemyScript.SetIsAttacking(false);
        }
    }
}
