using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement_X3 : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

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

    [Header("Air Control")]
    public float airControlMultiplier = 0.5f;

    [Header("Extra Skills")]
    public bool hasDash = false;
    public bool hasDoubleJump = false;

    private bool usedDoubleJump = false;
    private float lastDashTime = -999f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGroundedLastFrame;
    private bool hasJumped;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Transform cam;
    private float turnSmoothVelocity;
    private Animator animator;

    [Header("Dash Settings")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.3f;
    private bool isDashing = false;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();

        if (groundCheck == null)
            Debug.LogWarning("❗ groundCheck ไม่ได้เซ็ตใน Inspector");

        if (cam == null)
            Debug.LogWarning("❗ cam (Main Camera) ไม่ได้เซ็ตใน Inspector");

        if (animator == null)
            Debug.LogError("❌ ไม่พบ Animator ใน GameObject หรือลูกของมัน");
    }

    void Update()
    {
        // ตรวจสอบพื้น
        isGrounded = false;
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) ||
                         Physics.Raycast(transform.position, Vector3.down, out _, groundDistance + 0.2f, groundMask);
        }

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            if (!wasGroundedLastFrame)
            {
                hasJumped = false;
                usedDoubleJump = false;
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump Buffer + Trigger Jump_Up ทันที
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
            animator.SetTrigger("Jump_Up"); // ✅ เรียกทันทีเมื่อกด Spacebar
            animator.SetBool("isJumping", true);
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // รับ input เดิน
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 moveDirectionXZ = Vector3.zero;

        if (new Vector2(x, z).sqrMagnitude > 0.01f && cam != null)
        {
            float targetAngle = Mathf.Atan2(x, z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDirectionXZ = moveDir.normalized * moveSpeed;
        }

        if (!isGrounded)
            moveDirectionXZ *= airControlMultiplier;

        // วิ่ง
        animator.SetBool("isRunning", isGrounded && moveDirectionXZ.magnitude > 0.1f);

        // กระโดดแรก
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0 && !hasJumped)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpBufferCounter = 0f;
            hasJumped = true;
            usedDoubleJump = false;

            Debug.Log("✅ Jump!");
        }
        // กระโดดสอง
        else if (hasDoubleJump && !isGrounded && !usedDoubleJump && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            usedDoubleJump = true;

            animator.SetTrigger("Jump_Up");
            animator.SetBool("isJumping", true);
            Debug.Log("🟣 Double Jump!");
        }

        // แรงโน้มถ่วง
        if (velocity.y < 0)
        {
            animator.SetTrigger("Jump_Down");
            animator.SetBool("isJumping", true);
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

        if (isGrounded && animator.GetBool("isJumping"))
        {
            animator.SetBool("isJumping", false);
        }

        // Dash
        if (hasDash && Input.GetKeyDown(KeyCode.E) && Time.time > lastDashTime + 2f && !isDashing)
        {
            StartCoroutine(Dash());
        }

        // Move Final
        if (!isDashing)
        {
            Vector3 finalMove = new Vector3(moveDirectionXZ.x, velocity.y, moveDirectionXZ.z);
            controller.Move(finalMove * Time.deltaTime);
        }

        wasGroundedLastFrame = isGrounded;
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        Vector3 dashDir = transform.forward;
        float timer = 0f;

        animator.SetTrigger("Dash");
        Debug.Log("🌀 Start Dash!");

        while (timer < dashDuration)
        {
            controller.Move(dashDir * dashSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        Debug.Log("✅ Dash Ended");
    }
}
