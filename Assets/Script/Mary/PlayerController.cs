using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float turnSmoothTime = 0.065f;

    private PlayerInput input;
    private float turnMoveVelocity, targetAngle;
    private Transform camera;
    private Animator anim;

    // Start is called before the first frame update
    void Awake()
    {
        input = new PlayerInput();
        camera = Camera.main.transform;
        anim = GetComponentInChildren<Animator>();

        input.Player.Attack.performed += _ => Attack();
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
        Vector3 direction = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;
        if (direction.magnitude >= 0.1f)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;

            Vector3 moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
            transform.position = new Vector3(transform.position.x + moveDir.x * moveSpeed * Time.deltaTime,
                       transform.position.y, transform.position.z + moveDir.z * moveSpeed * Time.deltaTime);
        }

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnMoveVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

        // Animation
        anim.SetFloat("Speed", direction.magnitude);
    }

    private void Attack()
    {
        var attackScript = anim.GetBehaviours<Attack>();
        if (attackScript != null)
        {
            if (attackScript.)
            {
                anim.SetTrigger("Attack");
            }
        }
        else
        {
            Debug.Log("Attack script not found");
        }
    }
}
