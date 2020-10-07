using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public float stunTime;

}

[System.Serializable]
public struct EnemyEvent
{
    public UnityEvent onDetectingPlayer;
    public UnityEvent onMoving;
    public UnityEvent onTriggerEnter;
}

