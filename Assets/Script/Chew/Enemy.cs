using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using DG.Tweening;
using MyBox;

[RequireTag("Enemy")]
[RequireLayer("Enemy")]
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    //debugging parameter
    public Debuginput input;
    [ReadOnly] [SerializeField] private string currentState, lastState;
    //----

    [SerializeField] private EnemyStat enemyStat;
    private StateMachine<Enemy> stateMachine;
    private GameObject targetPlayer; //information of player
    private Rigidbody enemyRigidbody;
    private Collider collider;
    private ItemDropEvent dropableItem;
    private Animator anim;
    private float maxHealth;
    private Vector3 aoeAimPoint;

    [HideInInspector] public bool canAttack;
    [HideInInspector] public bool canMove;
    [HideInInspector] public bool isDeath;
    [HideInInspector] public bool isDying;
    [HideInInspector] public bool forceAttack;
    [HideInInspector] public bool isStun;
    public EnemyEvent enemyEvent;
    const float wallHitDistance = 0.5f;


    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        dropableItem = GetComponent<ItemDropEvent>();
        anim = GetComponentInChildren<Animator>();
        collider = GetComponent<Collider>();
        enemyRigidbody = GetComponent<Rigidbody>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player");

        stateMachine = new StateMachine<Enemy>();
        stateMachine.Setup(this, new EnemyMovement());
        currentState = stateMachine.GetCurrentState.ToString();

        isDeath = false;
        forceAttack = false;
        canAttack = true;
        canMove = true;
        isStun = false;
        maxHealth = enemyStat.health;
        if (enemyStat.attackType != AttackType.AreaMelee && enemyStat.attackType != AttackType.AreaRanged)
        {
            enemyStat.attackRadiusOfArea = 0;
        }

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

    public float GetWallHitDistance
    {
        get { return wallHitDistance; }
    }

    private void OnDrawGizmosSelected()
    {
        Quaternion rightRayRotation = Quaternion.AngleAxis(EnemyStat.attackAngle / 2, Vector3.up);
        Quaternion leftRayRotation = Quaternion.AngleAxis(-EnemyStat.attackAngle / 2, Vector3.up);
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position + Vector3.up, leftRayDirection * EnemyStat.attackRange);
        Gizmos.DrawRay(transform.position + Vector3.up, rightRayDirection * EnemyStat.attackRange);

    }

    public void ReceiveDamage(float damage)
    {
        if (isDeath)
        {
            return;
        }
        stateMachine.Setup(this, new EnemyGetHit());

        enemyStat.health = Mathf.Clamp(enemyStat.health - damage, 0.0f, maxHealth);

        Vector3 moveDirection = (targetPlayer.transform.position - transform.position).normalized;
        //enemyRigidbody.AddForce(moveDirection * -200f);

    }

    public float CheckPlayerDistance()    
    {
        return Vector3.Distance(transform.position, targetPlayer.transform.position);
    }

    public bool CheckWallHit(float maxDistance, bool reverseDir = false)
    {
        LayerMask wallMask = LayerMask.GetMask("Wall");
        RaycastHit wallRayHit = new RaycastHit();
        Ray forwardRay;
        if (reverseDir)
        {
            forwardRay = new Ray(transform.position + Vector3.up, -transform.forward);
        }
        else
        {
            forwardRay = new Ray(transform.position + Vector3.up, transform.forward);
        }
        if (Physics.Raycast(forwardRay, out wallRayHit, maxDistance, wallMask))
        {
            Debug.Log("Wall hit");
            return true;
        }
        return false;
    }

    public void MoveAwayFromPlayer()
    {
        Vector3 direction = (TargetPlayer.transform.position - transform.position).normalized;
        transform.position = new Vector3(transform.position.x + (-direction.x * EnemyStat.movementSpeed) * Time.deltaTime,
                         transform.position.y, transform.position.z + (-direction.z * EnemyStat.movementSpeed) * Time.deltaTime);


        Anim.SetFloat("Speed",EnemyStat.movementSpeed);
    }

    public void ChangeState(IState<Enemy> state)
    {
        stateMachine.ChangeState(state);
    }

    public bool CheckTargetInRange(float angle , float distance)
    {
        Vector3 origin = transform.position - (transform.forward * collider.bounds.extents.z);
        origin = new Vector3(origin.x, 0.0f, origin.z);
        Vector3 target = new Vector3(targetPlayer.transform.position.x, 0.0f, targetPlayer.transform.position.z);

        var dir = (target - origin).normalized;
        float calcAngle = Vector3.Angle(dir, transform.forward);
        var dist = Vector3.Distance(origin, target);
        if (Mathf.Abs(calcAngle) <= angle && dist <= distance)
        {
            return true;
        }
        return false;
    }

    public bool CheckTargetInRange(float angle, float distance, Vector3 pos)
    {
        Vector3 target = new Vector3(targetPlayer.transform.position.x, 0.0f, targetPlayer.transform.position.z);
        var dir = (target - pos).normalized;
        float calcAngle = Vector3.Angle(dir, transform.forward);
        var dist = Vector3.Distance(pos, target);
        if (Mathf.Abs(calcAngle) <= angle && dist <= distance)
        {
            return true;
        }
        return false;
    }

    public IEnumerator AoeAttack(float waitTime = 0.5f)
    {
        if (enemyStat.attackType != AttackType.AreaMelee && enemyStat.attackType != AttackType.AreaRanged)
        {
            yield break; 
        }
        aoeAimPoint = transform.position + ((targetPlayer.transform.position - transform.position).normalized * EnemyStat.attackRange);
        aoeAimPoint.y = 0.1f;
        var aoe = Instantiate(enemyStat.aoeIndicator, aoeAimPoint, enemyStat.aoeIndicator.transform.rotation) as GameObject;
        //radius of effect is set to 0.5f as default 
        aoe.transform.localScale *= (enemyStat.attackRadiusOfArea * 2);
        //wait the aoe circle to reach its maximum size before the effect occur
        Destroy(aoe, enemyStat.indicatorTime + waitTime);
        yield return new WaitForSeconds(waitTime);
        var aoeEffect = Instantiate(enemyStat.indicatorEffect, aoeAimPoint, enemyStat.indicatorEffect.transform.rotation) as GameObject;
        aoeEffect.transform.localScale *= (enemyStat.attackRadiusOfArea * 2);
        Destroy(aoeEffect, enemyStat.indicatorTime);
        yield return new WaitForSeconds(enemyStat.indicatorTime);
        DealDamage();
    }

    public void SpawnProjectile()
    {
        if (enemyStat.projectiles)
        {
            var proj = Instantiate(enemyStat.projectiles, transform.position + Vector3.up  + transform.forward, transform.rotation);
            proj.originTag = gameObject.tag;
        }
        else
        {
            Debug.LogWarning("Projectile not found");
        }
    }



    public void DealDamage(bool isProjectile =false)
    {
        bool ret = false;
        switch(enemyStat.attackType)
        {
            case AttackType.Melee:
                ret = CheckTargetInRange(EnemyStat.attackAngle / 2, EnemyStat.attackRange);
                break;
            case AttackType.AreaRanged:
                ret = CheckTargetInRange(360, enemyStat.attackRadiusOfArea, aoeAimPoint);
                break;
            case AttackType.AreaMelee:
                ret = CheckTargetInRange(360, enemyStat.attackRadiusOfArea);
                break;
        }
        if (ret)
        {
            targetPlayer.GetComponent<PlayerController>().TakeDamage(1, transform);
        }
    }

    public void FaceDirection(Vector3 towards,bool reverse = false, float smoothtime = 0.05f)
    {
        if (reverse)
        {
            towards = 2 * transform.position - towards;
        }
        transform.DOLookAt(towards, smoothtime,AxisConstraint.Y);

    }
}
