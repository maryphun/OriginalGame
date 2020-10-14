using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using MyBox;

public enum AttackType
{
    Melee,
    Ranged,
    AreaMelee
};

[System.Serializable]
public class EnemyStat
{
    public float health;
    public float movementSpeed;
    public AttackType attackType;
    public float attackDelay;
    public float attackRange;
    public float attackAngle;
    [ConditionalField(nameof(attackType),false,AttackType.AreaMelee)]
    public float attackRadiusOfArea;
    [ConditionalField(nameof(attackType), false, AttackType.Ranged)]
    public Projectiles projectiles;
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

