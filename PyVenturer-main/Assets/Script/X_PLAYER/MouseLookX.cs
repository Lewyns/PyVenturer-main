using UnityEngine;

public class MouseLookX : MonoBehaviour
{
    public float mouseSensitivity = 30f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // หมุนกล้องแนวตั้ง
        playerBody.Rotate(Vector3.up * mouseX); // หมุนตัวผู้เล่นแนวนอน

        // กด Escape เพื่อปลดล็อกเมาส์ (เช่น ใช้ในเมนู)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}