using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementX : MonoBehaviour
{
    // ────────────────────── Movement ──────────────────────
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Sprint")]
    public float sprintSpeed = 8f;
    private float currentSpeed;

    // ─────────────────────── Dash ─────────────────────────
    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool canAirDash = true;

    private bool isDashing = false;
    private float dashTimer;
    private float dashCooldownTimer = 0f;
    private Vector3 dashDirection;

    // ─────────────────────── Jump ─────────────────────────
    [Header("Jump")]
    public float jumpHeight = 2f;
    public float gravity = -20f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Coyote Time")]
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("Jump Buffer")]
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    // ────────────────────── Ground ────────────────────────
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool hasJumped;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // ──── NEW: ยึดติดแพลตฟอร์ม ────────────────────────────
    private Transform currentPlatform;          // แพลตฟอร์มที่กำลังยืนอยู่

    // ──────────────────── Unity Events ────────────────────

    public Transform cam; // กล้องที่ใช้หมุน (Main Camera หรือ CameraPivot)
    private float turnSmoothVelocity; // สำหรับหมุนเนียนๆ


    void Start()
    {
        controller = GetComponent<CharacterController>();

        controller.slopeLimit = 45f;
        controller.stepOffset = 0.3f;
        controller.skinWidth = 0.08f;
    }

    void Update()
    {
        // เช็กพื้นด้วย CheckSphere และ Raycast
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) ||
                     Physics.Raycast(transform.position, Vector3.down, out _, groundDistance + 0.2f, groundMask);

        if (!isGrounded && currentPlatform != null)
        {
            transform.SetParent(null);
            currentPlatform = null;
        }

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            canAirDash = true;
            hasJumped = false;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        Vector3 inputDir = new Vector3(x, 0f, z).normalized;

        if (inputDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);

            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);  // หมุนตัวละครตามมุมที่คำนวณ

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }
        else
        {
            // เมื่อปล่อยปุ่มให้หยุดการหมุนทันที
            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

            // หยุดการเคลื่อนที่ในทิศทาง X และ Z
            velocity.x = 0f;
            velocity.z = 0f;
        }

        if (dashCooldownTimer > 0) dashCooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.E) && dashCooldownTimer <= 0f && !isDashing)
        {
            if (isGrounded || canAirDash)
            {
                isDashing = true;
                Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f,
                                                     Input.GetAxisRaw("Vertical")).normalized;
                dashDirection = inputDirection.magnitude > 0
                                ? Quaternion.Euler(0f, cam.eulerAngles.y, 0f) * inputDirection
                                : transform.forward;
                dashTimer = dashDuration;
                dashCooldownTimer = dashCooldown;
                if (!isGrounded) canAirDash = false;
            }
        }

        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0 && !hasJumped)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpBufferCounter = 0f;
            hasJumped = true;
        }

        if (velocity.y < 0)
            velocity.y += gravity * fallMultiplier * Time.deltaTime;
        else if (velocity.y > 0 && !Input.GetButton("Jump"))
            velocity.y += gravity * lowJumpMultiplier * Time.deltaTime;
        else
            velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (isDashing)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f) isDashing = false;
        }
    }

    // ───────── NEW: เริ่มเกาะแพลตฟอร์มเมื่อเหยียบด้านบน ─────────
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("MovingPlatform") &&
            Vector3.Dot(hit.normal, Vector3.up) > 0.5f)     // ชนด้านบน
        {
            transform.SetParent(hit.collider.transform);     // เกาะแพลตฟอร์ม
            currentPlatform = hit.collider.transform;
        }
    }
}
