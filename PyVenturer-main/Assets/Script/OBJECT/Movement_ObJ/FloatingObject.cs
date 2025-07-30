using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("ตั้งค่าการลอย")]
    public float floatAmplitude = 0.5f; // ความสูงของการลอย (ขึ้น/ลง)
    public float floatSpeed = 1f;       // ความเร็วในการลอย

    private Vector3 startPosition;

    void Start()
    {
        // บันทึกตำแหน่งเริ่มต้นไว้ก่อน
        startPosition = transform.position;
    }

    void Update()
    {
        // คำนวณตำแหน่งใหม่โดยใช้ sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
