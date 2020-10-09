using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public Debuginput input;
    [SerializeField]
    private EnemyStat enemyStat;
    [ReadOnly]
    [SerializeField]
    private string currentState;
    private StateMachine<Enemy> stateMachine;
    private GameObject targetPlayer;
    public EnemyEvent enemyEvent;
    private Rigidbody enemyRigidbody;
    private bool isDeath;
    private ItemDropEvent dropableItem;
    private Animator anim;
    private bool isAttacking;
    [HideInInspector] public bool canAttack;


    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        dropableItem = GetComponent<ItemDropEvent>();
        anim = GetComponentInChildren<Animator>();

        isDeath = false;
        targetPlayer = GameObject.FindGameObjectWithTag("Player");
        stateMachine = new StateMachine<Enemy>();
        stateMachine.Setup(this, new EnemyMovement());
        currentState = stateMachine.GetCurrentState.ToString();
        enemyRigidbody = GetComponent<Rigidbody>();
        canAttack = true;

        input = new Debuginput();
        input.Enable();

        input.debugging.EnemyKnockback.performed += _ => ReceiveDamage();
        input.debugging.DropItem.performed += _ => dropableItem.DropItem();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyStat.health <= 0 /*&& !isDeath*/)
        {
            isDeath = true;
            stateMachine.ChangeState(new EnemyDeath());
        }
        
        stateMachine.Update();
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
        Gizmos.DrawRay(transform.position, leftRayDirection * enemyStat.visionRadius);
        Gizmos.DrawRay(transform.position, rightRayDirection * enemyStat.visionRadius);
    }

    public void ReceiveDamage(Collider damageOwner = null)
    {
        stateMachine.Setup(this, new EnemyGetHit());
        if (damageOwner)
        {
            enemyStat.health -= damageOwner.GetComponent<Damage>().damageValue;
        }
        Vector3 moveDirection = (targetPlayer.transform.position - transform.position).normalized;
        enemyRigidbody.AddForce(moveDirection * -500f);
    
    }

    public float CheckDistance()
    {
        return Vector3.Distance(transform.position, targetPlayer.transform.position);
    }

    public void ChangeState(IState<Enemy> state)
    {
        stateMachine.ChangeState(state);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            ReceiveDamage(other);
        }
    }
}
