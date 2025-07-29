using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement_X2 : MonoBehaviour
{
    // ────────────────────── Movement ──────────────────────
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Sprint")]
    public float sprintSpeed = 8f;
    private float currentSpeed;

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

    [Header("Air Control")]
    public float airControlMultiplier = 0.5f;

    // ────────────────────── Ground ────────────────────────
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGroundedLastFrame; // ✅ เพิ่มมาใหม่
    private bool hasJumped;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    // ──────────────────── Unity Events ────────────────────
    public Transform cam;
    private float turnSmoothVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        controller.slopeLimit = 45f;
        controller.stepOffset = 0.3f;
        controller.skinWidth = 0.08f;

        if (groundCheck == null)
            Debug.LogWarning("❗ groundCheck ไม่ได้เซ็ตใน Inspector");

        if (cam == null)
            Debug.LogWarning("❗ cam (Main Camera) ไม่ได้เซ็ตใน Inspector");
    }

    void Update()
    {
        // 🔍 ตรวจสอบพื้น
        isGrounded = false;
        if (groundCheck != null)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) ||
                         Physics.Raycast(transform.position, Vector3.down, out _, groundDistance + 0.2f, groundMask);
        }

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;

            // ✅ รีเซ็ต hasJumped เฉพาะตอน "แตะพื้นจากกลางอากาศ"
            if (!wasGroundedLastFrame)
                hasJumped = false;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // 🔁 Jump Buffer
        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // 📦 รับ input
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        Vector3 moveDirectionXZ = Vector3.zero;

        if (new Vector2(x, z).sqrMagnitude > 0.01f && cam != null)
        {
            float targetAngle = Mathf.Atan2(x, z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);

            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDirectionXZ = moveDir.normalized * currentSpeed;
        }

        // ✅ ลดการควบคุมกลางอากาศ
        if (!isGrounded)
            moveDirectionXZ *= airControlMultiplier;

        // 🔼 กระโดด
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0 && !hasJumped)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpBufferCounter = 0f;
            hasJumped = true;

            Debug.Log("✅ Jump Triggered!");
        }

        // ⬇️ แรงโน้มถ่วง
        if (velocity.y < 0)
            velocity.y += gravity * fallMultiplier * Time.deltaTime;
        else if (velocity.y > 0 && !Input.GetButton("Jump"))
            velocity.y += gravity * lowJumpMultiplier * Time.deltaTime;
        else
            velocity.y += gravity * Time.deltaTime;

        // ✅ รวมการเคลื่อนที่
        Vector3 finalMove = new Vector3(moveDirectionXZ.x, velocity.y, moveDirectionXZ.z);
        controller.Move(finalMove * Time.deltaTime);

        // ✅ เก็บสถานะพื้นไว้ใช้รอบหน้า
        wasGroundedLastFrame = isGrounded;

        // 🧪 Debug log
        Debug.Log($"isGrounded = {isGrounded}, moveXZ = {moveDirectionXZ}, velY = {velocity.y:F2}");
    }
}
