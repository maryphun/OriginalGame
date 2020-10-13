using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

public enum AttackType
{
    Melee,
    Ranged,
    AreaMelee
};

[System.Serializable]
public struct EnemyStat
{
    public float health;
    public float movementSpeed;
    public AttackType attackType;
    public float attackDelay;
    public float attackRange;
    public float attackAngle;
    public float visionRadius;
    public float visionAngle;
    public float stunTime;
    public float escapeRange;
}

[System.Serializable]
public struct EnemyEvent
{
    public UnityEvent onDetectingPlayer;
    public UnityEvent onMoving;
    public UnityEvent onTriggerEnter;
}


