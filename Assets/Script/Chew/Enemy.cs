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
    [HideInInspector] public bool canMove;
    private float maxHealth;

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
        Gizmos.DrawRay(transform.position + Vector3.up, leftRayDirection * enemyStat.visionRadius);
        Gizmos.DrawRay(transform.position + Vector3.up, rightRayDirection * enemyStat.visionRadius);
    }

    public void ReceiveDamage(float damage)
    {
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

    public bool DetectObject(Transform origin, Transform target,float visionAngle,float visionDistance)
    {
        var dir = (target.position - origin.position).normalized;
        float angle = Vector3.Angle(dir, origin.forward);
        var dist = Vector3.Distance(origin.position, target.position);
        if (Mathf.Abs(angle) < visionAngle && dist < visionDistance)
        {
            return true;
        }
        return false;
    }

    public void DealDamage()
    {
        if (DetectObject(transform, TargetPlayer.transform, EnemyStat.visionAngle / 2, EnemyStat.attackRange))
        {
            targetPlayer.GetComponent<PlayerController>().TakeDamage(1, transform);
        }

    }
}
