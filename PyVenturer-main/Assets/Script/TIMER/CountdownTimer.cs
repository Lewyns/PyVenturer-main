using UnityEngine;
using TMPro; // ใช้สำหรับ TextMeshPro

public class CountdownTimer : MonoBehaviour
{
    public TMP_Text timeText; // ← ลาก TextMeshPro UI (TIME LEFT) มาใส่ใน Inspector

    private float timeLeft = 150f + 1f; // เริ่มต้นที่ 2 นาที 30 วินาที (150 วินาที) บวกไปอีก 1 วิ

    void Update()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0) timeLeft = 0;
            UpdateTimeText();
        }
    }

    void UpdateTimeText()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft % 60f);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // เช่น 02:10
    }
}
