using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Melee,
    Ranged
};

[System.Serializable]
public struct EnemyStat
{
    public float health;
    public float movementSpeed;
    public AttackType attackType;
    public float attackRange;
    public float visionRadius;
    public float visionAngle;
  
}

