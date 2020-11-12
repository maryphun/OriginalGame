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
    AreaMelee,
    AreaRanged,
    Golem
};

[System.Serializable]
public class EnemyStat
{
    [PositiveValueOnly] public float health;
    [PositiveValueOnly] public float movementSpeed;
    [PositiveValueOnly] public float attackDelay;
    [PositiveValueOnly] public float attackRange;
    [PositiveValueOnly] public float visionRadius;
    [PositiveValueOnly] public float visionAngle;
    [PositiveValueOnly] public float stunTime;
    [PositiveValueOnly] public float escapeRange; //distance required to run away from player

    public AttackType attackType;
    [ConditionalField(nameof(attackType), false, AttackType.Melee)]
    [PositiveValueOnly]
    public float attackAngle;   //angle required to form cone for attack area
    [ConditionalField(nameof(attackType),false,AttackType.AreaMelee,AttackType.AreaRanged)]
    [PositiveValueOnly]
    public float attackRadiusOfArea,indicatorTime;
    [ConditionalField(nameof(attackType), false, AttackType.AreaMelee, AttackType.AreaRanged)]
    public GameObject aoeIndicator,indicatorEffect;
    [ConditionalField(nameof(attackType), false, AttackType.Ranged,AttackType.AreaRanged)]
    public GameObject projectiles;
    [ConditionalField(nameof(attackType), false, AttackType.Ranged, AttackType.AreaRanged)]
    public bool followTarget;
}

[System.Serializable]
public struct EnemyEvent
{
    public UnityEvent onDetectingPlayer;
    public UnityEvent onMoving;
    public UnityEvent onTriggerEnter;
}

