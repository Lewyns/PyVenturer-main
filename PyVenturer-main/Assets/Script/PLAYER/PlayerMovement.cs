using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Sprint")]
    public float sprintSpeed = 8f;        // ความเร็วตอนวิ่ง
    private float currentSpeed;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool canAirDash = true;


    private bool isDashing = false;
    private float dashTimer;
    private float dashCooldownTimer = 0f;
    private Vector3 dashDirection;

    [Header("Jump")]
    public float jumpHeight = 2f;
    public float gravity = -20f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Coyote Time")]
    public float coyoteTime = 200f;
    private float coyoteTimeCounter;

    [Header("Jump Buffer")]
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private CharacterController controller;
    private Vector3 velocity;

    private bool isGrounded;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Coyote time logic
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump buffer logic
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        // กด Shift เพื่อวิ่ง
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Update dash cooldown
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // Dash Input
        // Dash Input
        if (Input.GetKeyDown(KeyCode.E) && dashCooldownTimer <= 0f && !isDashing)
        {
            if (isGrounded || canAirDash)
            {
                isDashing = true;

                // ✅ คำนวณทิศทางตาม WASD
                Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

                if (inputDirection.magnitude > 0)
                {
                    dashDirection = transform.TransformDirection(inputDirection);
                }
                else
                {
                    dashDirection = transform.forward;
                }

                dashTimer = dashDuration;
                dashCooldownTimer = dashCooldown;

                if (!isGrounded)
                {
                    canAirDash = false;
                }
            }
        }




        // Jump
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpBufferCounter = 0f;
        }

        // Gravity
        if (velocity.y < 0)
        {
            velocity.y += gravity * fallMultiplier * Time.deltaTime;
        }
        else if (velocity.y > 0 && !Input.GetButton("Jump"))
        {
            velocity.y += gravity * lowJumpMultiplier * Time.deltaTime;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            canAirDash = true; // รีเซ็ตการ dash กลางอากาศเมื่อแตะพื้น
        }


        // Dash mechanic
        if (isDashing)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }

    }
}
