using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float turnSmoothTime = 0.065f;

    private PlayerInput input;
    private float turnMoveVelocity, targetAngle, moveSpeedMax;
    private new Transform camera;
    private Animator anim;
    private bool isAttacking;

    // Start is called before the first frame update
    void Awake()
    {
        input = new PlayerInput();
        camera = Camera.main.transform;
        anim = GetComponentInChildren<Animator>();

        input.Player.Attack.performed += _ => Attack();

        // value initialization
        moveSpeedMax = moveSpeed;
        isAttacking = false;
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

        // check condition if the player could move
        if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Move(moveInput);
        }

        Rotate();
    }

    private void Attack()
    {
        var attackScript = anim.GetBehaviour<Attack>();
        if (attackScript != null)
        {
            if (attackScript.canAttack)
            {
                anim.SetTrigger("Attack");
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
                    isAttacking = true;
                }
            }
        }
        else
        {
            Debug.Log("Attack script not found");
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
            transform.position = new Vector3(transform.position.x + moveDir.x * moveMagnitude * Time.deltaTime,
                       transform.position.y, transform.position.z + moveDir.z * moveMagnitude * Time.deltaTime);
        }

        // Animation
        anim.SetFloat("Speed", direction.magnitude);
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
}
