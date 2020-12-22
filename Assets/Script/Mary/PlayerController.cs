using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField, Range(1, 27)] int initiateHitPoint = 3;
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float turnSmoothTime = 0.065f;
    [SerializeField] float comboResetTime = 0.333f;
    [SerializeField] float dashDistance = 2f;
    [SerializeField] float dashTime = 0.5f;
    [SerializeField] float[] attackAnimFrame;
    [SerializeField] float damageRange = 1.5f;
    [SerializeField] float damageAngle = 45;
    [SerializeField] float invulnerableTime = 0.75f;
    [SerializeField] float attackBaseDamage = 10f;
    [SerializeField] LayerMask wall;
    [SerializeField] private ProjectileManager projectilescript;

    private PlayerInput input;
    private float turnMoveVelocity, targetAngle, moveSpeedMax, attackAngle, comboTimerCount;
    private int comboCount;
    private new Transform camera;
    private PlayerAnimator visualScript;
    private Animator anim;
    private bool isAttacking, shouldAttack, canRegisterAttack;
    private bool isDashing, canRegisterDash;
    private Collider collider;
    private List<Transform> attackedEnemy = new List<Transform>();
    private List<Transform> immuneEnemy = new List<Transform>();
    private HitPoint hpbar;
    [HideInInspector] public bool canAttack;

    // Start is called before the first frame update
    void Awake()
    {
        // references initialization
        input = new PlayerInput();
        camera = Camera.main.transform;
        anim = GetComponentInChildren<Animator>();
        visualScript = GetComponentInChildren<PlayerAnimator>();
        collider = GetComponent<Collider>();
        hpbar = GameObject.FindGameObjectWithTag("Hpbar").GetComponent<HitPoint>();

        // key registration
        input.Player.Attack.performed += _ => Attack();
        input.Player.Dash.performed += _ => Dash(new Vector2(input.Player.HorizontalMove.ReadValue<float>(), input.Player.VerticalMove.ReadValue<float>()));

        // value initialization
        moveSpeedMax = moveSpeed;
        isAttacking = false;
        attackedEnemy.Clear();
        immuneEnemy.Clear();
        shouldAttack = false;
        canAttack = false;
        canRegisterAttack = false;
        isDashing = false;
        canRegisterDash = false;

        //debug
        // Time.timeScale = 0.2f;
    }
    
    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void SetInputAction(bool enable)
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(OptionManager.isPaused)
        {
            if(input.asset.enabled)
            {
                input.Disable();
            }
        }
        else
        {
            if(!input.asset.enabled)
            {
                input.Enable();
            }
        }
        if (IsDeath()) return;

        // read input
        Vector2 moveInput = new Vector2(input.Player.HorizontalMove.ReadValue<float>(), input.Player.VerticalMove.ReadValue<float>());
        comboTimerCount = Mathf.Clamp(comboTimerCount - Time.deltaTime, 0.0f, comboResetTime);

        // check condition if the player could move
        if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !anim.GetCurrentAnimatorStateInfo(0).IsTag("Dash"))
        {
            Move(moveInput);
        }

        Rotate();
    }

    private void Dash(Vector3 moveInput)
    {
        Vector3 direction = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;
        if (direction.magnitude != 0.0f && !isDashing && canRegisterDash)
        {
            anim.Play("Dash", 0, 0.10f);
            moveSpeedMax = 0.0f;
            isDashing = true;

            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            Vector3 moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;

            Debug.DrawLine(new Vector3(transform.position.x, 1.0f, transform.position.z),
                new Vector3(transform.position.x + moveDir.x, 1.0f, transform.position.z + moveDir.z), Color.red, 1.0f);
            MoveWithRayCast(moveDir, dashDistance, dashTime);
        }
    }

    private void Attack()
    {
        if (canAttack)
        {
            if (comboTimerCount <= 0.0f)
            {
                comboCount = 0;
            }
            comboCount++;
            if (comboCount >= 4)
            {
                comboCount = 1;
            }
            comboTimerCount = comboResetTime;
            anim.SetInteger("Combo", comboCount);
            anim.Play("attack_0" + comboCount.ToString(), 0, attackAnimFrame[comboCount - 1]);
            canAttack = false;
            canRegisterAttack = false;
            canRegisterDash = false;
            if (isDashing)
            {
                isDashing = false;
            }
            moveSpeedMax = 0.0f;
            attackedEnemy.Clear();

            Ray ray = camera.GetComponent<CameraFollow>().MousePositionPointToRay();
            Plane groundplane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundplane.Raycast(ray, out rayLength))
            {
                Vector3 pointToLook = ray.GetPoint(rayLength);
                Debug.DrawLine(ray.origin, pointToLook, Color.red, 1.0f);

                var tmp = (pointToLook - transform.position).normalized;
                targetAngle = Mathf.Atan2(tmp.x, tmp.z) * Mathf.Rad2Deg;
                attackAngle = targetAngle;
                isAttacking = true;
            }
        }
        else
        {
            if (canRegisterAttack)
            {
                shouldAttack = true;
            }
        }
    }

    private void Move(Vector3 keyinput)
    {
        if (isAttacking) { return; }

        Vector3 direction = new Vector3(keyinput.x, 0.0f, keyinput.y).normalized;
        if (direction.magnitude >= 0.1f)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;

            // limit the move speed
            float moveMagnitude = Mathf.Clamp(moveSpeed, 0.0f, moveSpeedMax);

            Vector3 moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;

            MoveWithColliderCheck(moveDir, moveMagnitude * Time.deltaTime, 0f);
            //transform.position = new Vector3(transform.position.x + moveDir.x * moveMagnitude * Time.deltaTime,
            //           transform.position.y, transform.position.z + moveDir.z * moveMagnitude * Time.deltaTime);
        }

        // Animation
        anim.SetFloat("Speed", direction.magnitude);
    }

    public void Move(float distance)
    {
        Vector3 moveDir = (Quaternion.Euler(0f, attackAngle, 0f) * Vector3.forward).normalized;
        MoveWithRayCast(moveDir, distance, 0.1f);
    }

    public void ResetMoveSpeedMax()
    {
        moveSpeedMax = moveSpeed;
    }

    private void Rotate()
    {
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnMoveVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    public void SetIsAttacking(bool boolean)
    {
        isAttacking = boolean;
    }

    public void AttackEnd()
    {
        canRegisterDash = true;
        if (shouldAttack)
        {
            shouldAttack = false;
            Attack();
        }
    }

    public void Dashing(bool boolean)
    {
        isDashing = boolean;
    }

    public void StartRegisterAttack()
    {
        canRegisterAttack = true;
    }

    public void MoveWithRayCast(Vector3 normalizedVector, float distance, float time)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, normalizedVector, out hit, distance, wall))
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

    public void MoveWithColliderCheck(Vector3 normalizedVector, float distance, float time)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, normalizedVector, out hit, distance + collider.bounds.extents.z, wall))
        {
            return;
        }
        transform.DOMove(transform.position + normalizedVector * distance, time, false);
    }

    public void DealDamage()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (IsTargetInAttackRange(enemy.transform) && !attackedEnemy.Contains(enemy.transform))
            {
                attackedEnemy.Add(enemy.transform);
                visualScript.HitFX(Random.Range(0, 2), enemy.transform.position + Vector3.up);

                EnemyHitPoint enemyScript = enemy.GetComponent<EnemyHitPoint>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(attackBaseDamage);
                }
                else
                {
                    Debug.LogWarning("Enemy Properties script not found in attacked enemy. this enemy won't take damage and show hp bar.");
                }
            }
        }
    }

    public bool IsTargetInAttackRange(Transform enemy)
    {
        // Vectors
        Vector3 origin = transform.position - (transform.forward * collider.bounds.extents.z);
        origin = new Vector3(origin.x, 0.0f, origin.z);
        Vector3 target = new Vector3(enemy.position.x, 0.0f, enemy.position.z);

        // debug visualization
        Quaternion rightRayRotation = Quaternion.AngleAxis(damageAngle / 2, Vector3.up);
        Quaternion leftRayRotation = Quaternion.AngleAxis(-damageAngle / 2, Vector3.up);
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Debug.DrawRay(origin, leftRayDirection * damageRange, Color.white, 0.5f);
        Debug.DrawRay(origin, rightRayDirection * damageRange, Color.white, 0.5f);
        // ----------------------

        var dir = (target - origin).normalized;
        float angle = Vector3.Angle(dir, transform.forward);
        var dist = Vector3.Distance(origin, target);
        if (Mathf.Abs(angle) < damageAngle && dist < damageRange)
        {
            return true;
        }
        return false;
    }

    public void Heal(int value)
    {
        hpbar.ChangeHp(value);
    }

    public void TakeDamage(int value, Transform source)
    {
        if (IsDeath() || IsImmumeTarget(source)) return;

        hpbar.ChangeHp(-value);

        // Check if character is death again after taking damage
        if (IsDeath())
        {
            anim.Play("sofuckingded", 0, 0.2f);

            // Shouldn't be allowed to do these action
            canAttack = false;
            canRegisterAttack = false;
            canRegisterDash = false;
            moveSpeed = 0.0f;
        }
        else
        {
            /// Character survived this attack.

            // immune attack from this target for a period of time
            StartCoroutine(ImmuneDamageFromTarget(source, invulnerableTime));

            // Camera Shake
            StartCoroutine(camera.GetComponent<CameraFollow>().CameraShake(0.05f, 0.05f));

            if (!isAttacking && !isDashing)
            {
                anim.Play("Damaged", 0, 0.05f);
            }
        }
    }

    public void Callback()
    {
        Debug.Log("callback called");
    }

    public bool IsDeath()
    {
        // Character is considered death if hp equal to 0
        return hpbar.GetCurrentHP() == 0;
    }

    private IEnumerator ImmuneDamageFromTarget(Transform target, float time)
    {
        immuneEnemy.Add(target);
        yield return new WaitForSeconds(time);
        immuneEnemy.Remove(target);
    }

    public bool IsImmumeTarget(Transform target)
    {
        return immuneEnemy.Contains(target);
    }

    public void PlayAnim(string targetanim, int layer, float normalizedTime)
    {
        anim.Play(targetanim, layer, normalizedTime);
    }

    public void VariableInitialization()
    {
        // value initialization
        moveSpeedMax = moveSpeed;
        isAttacking = false;
        attackedEnemy.Clear();
        immuneEnemy.Clear();
        shouldAttack = false;
        canAttack = true;
        canRegisterAttack = false;
        isDashing = false;
        canRegisterDash = true;
        
        hpbar.ChangeHp(initiateHitPoint);
    }
}
