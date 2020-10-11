using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float turnSmoothTime = 0.065f;
    [SerializeField] float comboResetTime = 0.333f;
    [SerializeField] float dashDistance = 2f;
    [SerializeField] float dashTime = 0.5f;
    [SerializeField] float[] attackAnimFrame;
    [SerializeField] LayerMask wall;

    private PlayerInput input;
    private float turnMoveVelocity, targetAngle, moveSpeedMax, attackAngle, comboTimerCount;
    private int comboCount;
    private new Transform camera;
    private Animator anim;
    private bool isAttacking, shouldAttack, canRegisterAttack;
    private bool isDashing, canRegisterDash;
    private Collider collider;
    [HideInInspector] public bool canAttack;

    // Start is called before the first frame update
    void Awake()
    {
        input = new PlayerInput();
        camera = Camera.main.transform;
        anim = GetComponentInChildren<Animator>();
        collider = GetComponent<Collider>();
        shouldAttack = false;
        canAttack = true;
        canRegisterAttack = false;
        isDashing = false;
        canRegisterDash = true;

        input.Player.Attack.performed += _ => Attack();
        input.Player.Dash.performed += _ => Dash(new Vector2(input.Player.HorizontalMove.ReadValue<float>(), input.Player.VerticalMove.ReadValue<float>()));

        // value initialization
        moveSpeedMax = moveSpeed;
        isAttacking = false;

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

    // Update is called once per frame
    void Update()
    {
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
}
