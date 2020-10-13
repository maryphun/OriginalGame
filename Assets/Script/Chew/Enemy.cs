﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public Debuginput input;
    [SerializeField]
    private EnemyStat enemyStat;
    [ReadOnly]
    [SerializeField]
    private string currentState;
    [ReadOnly]
    public string lastState;
    private StateMachine<Enemy> stateMachine;
    private GameObject targetPlayer;
    public EnemyEvent enemyEvent;
    private Rigidbody enemyRigidbody;
    private Collider collider;
    private ItemDropEvent dropableItem;
    private Animator anim;
    private bool isAttacking;
    [HideInInspector] public bool canAttack;
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool isDeath;
    [HideInInspector] public bool isDying;
    private float maxHealth;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        dropableItem = GetComponent<ItemDropEvent>();
        anim = GetComponentInChildren<Animator>();
        collider = GetComponent<Collider>();

        isDeath = false;
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        stateMachine = new StateMachine<Enemy>();
        stateMachine.Setup(this, new EnemyMovement());
        currentState = stateMachine.GetCurrentState.ToString();
        enemyRigidbody = GetComponent<Rigidbody>();
        canAttack = true;
        canMove = true;
        maxHealth = enemyStat.health;

        input = new Debuginput();
        input.Enable();

        input.debugging.DropItem.performed += _ => dropableItem.DropItem();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyStat.health <= 0 && !isDeath)
        {
            isDeath = true;
            stateMachine.ChangeState(new EnemyDeath());
        }
        if( targetPlayer.GetComponent<PlayerController>().IsDeath())
        {
            Anim.SetFloat("Speed", 0f);
            stateMachine.ChangeState(new EnemyIdle());
        }
        
        stateMachine.Update();
        if (stateMachine.GetLastState != null)
        {
            lastState = stateMachine.GetLastState.ToString();
        }
        currentState = stateMachine.GetCurrentState.ToString();
     
    }

    public EnemyStat EnemyStat
    {
        get { return enemyStat; }
    }

    public GameObject TargetPlayer
    {
        get { return targetPlayer; }
    }
    public string CurrentState
    {
        get { return currentState; }
    }

    public Animator Anim
    {
        get { return anim; }
    }

    private void OnDrawGizmosSelected()
    {
        Quaternion rightRayRotation = Quaternion.AngleAxis(enemyStat.visionAngle/2, Vector3.up);
        Quaternion leftRayRotation = Quaternion.AngleAxis(-enemyStat.visionAngle/2, Vector3.up);
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position + Vector3.up, leftRayDirection * enemyStat.visionRadius);
        Gizmos.DrawRay(transform.position + Vector3.up, rightRayDirection * enemyStat.visionRadius);
    }

    public void ReceiveDamage(float damage)
    {
        if (isDeath)
        {
            return;
        }
        stateMachine.Setup(this, new EnemyGetHit());

        enemyStat.health = Mathf.Clamp(enemyStat.health - damage, 0.0f, maxHealth);

       // Vector3 moveDirection = (targetPlayer.transform.position - transform.position).normalized;
       // enemyRigidbody.AddForce(moveDirection * -500f);
    
    }

    public float CheckDistance()    
    {
        return Vector3.Distance(transform.position, targetPlayer.transform.position);
    }

    public void ChangeState(IState<Enemy> state)
    {
        stateMachine.ChangeState(state);
    }

    public bool DetectObject(float angle , float distance)
    {
        // Vectors
        Vector3 origin = transform.position - (transform.forward * collider.bounds.extents.z);
        origin = new Vector3(origin.x, 0.0f, origin.z);
        Vector3 target = new Vector3(targetPlayer.transform.position.x, 0.0f, targetPlayer.transform.position.z);

        var dir = (target - origin).normalized;
        float calcAngle = Vector3.Angle(dir, transform.forward);
        var dist = Vector3.Distance(origin, target);
        if (Mathf.Abs(calcAngle) < angle && dist < distance)
        {
            return true;
        }
        return false;
    }

    public bool DetectObjectInArea()
    {
        Vector3 origin = transform.position - (transform.forward * collider.bounds.extents.z);
        origin = new Vector3(origin.x, 0.0f, origin.z);
        Vector3 target = new Vector3(targetPlayer.transform.position.x, 0.0f, targetPlayer.transform.position.z);

        var dir = (target - origin).normalized;
        Vector3 attPoint = transform.position + (transform.forward * EnemyStat.attackRange / 2);
        Debug.DrawLine(attPoint - transform.forward * (enemyStat.attackRadiusOfArea / 2), attPoint + transform.forward * (enemyStat.attackRadiusOfArea / 2), Color.red, 2f);

        if (Vector3.Distance(attPoint,target) <= enemyStat.attackRadiusOfArea)
        {
            return true;
        }

        return false;
    }

    public void DealDamage()
    {
        //if (DetectObject(EnemyStat.attackAngle / 2, EnemyStat.attackRange))
        //{
        //    targetPlayer.GetComponent<PlayerController>().TakeDamage(1, transform);
        //}
        if (DetectObjectInArea())
        {
            targetPlayer.GetComponent<PlayerController>().TakeDamage(1,transform);
        }
    }

    public void MoveWithRayCast(Vector3 normalizedVector, float distance, float time)
    {
        LayerMask wallMask = LayerMask.GetMask("Wall");
        LayerMask playerMask = LayerMask.GetMask("Player");
        var finalmask = wallMask | playerMask;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, normalizedVector, out hit, distance, finalmask))
        {
            //New Distance 
            float newDistance = Vector3.Distance(hit.point, transform.position);
            if (collider.bounds.extents.z > newDistance)
            {
                return;
            }
            transform.DOMove(hit.point, time, false);
            return;
        }
        transform.DOMove(transform.position + normalizedVector * distance, time, false);
    }
}
