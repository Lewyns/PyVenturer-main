using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ScoreTimerManager : MonoBehaviour
{
    [Header("เวลา")]
    public TMP_Text timeText;             // TextMeshPro UI สำหรับแสดง TIME LEFT
    public float startTime = 150f;        // เวลานับถอยหลังเริ่มต้น (2 นาที 30 วินาที)
    private float timeLeft;               // เวลาที่เหลือในรอบปัจจุบัน

    [Header("คะแนน")]
    public TMP_Text SCORE_PERCENT;        // TextMeshPro UI สำหรับแสดงเปอร์เซ็นต์ SCORE
    public Slider ScoreSlider;            // Slider UI สำหรับแถบคะแนน
    private float score = 100f;           // คะแนนเริ่มต้น

    [Header("Player & Checkpoint")]
    public GameObject playerObject;       // GameObject ของผู้เล่น (ที่มี PlayerRespawn.cs)
    private PlayerRespawn playerRespawn;  // ตัวแปรอ้างถึงสคริปต์ PlayerRespawn

    private float savedTimeLeft;          // เก็บค่าเวลาเริ่มต้นไว้ใช้รีเซ็ต
    private float savedScore;             // เก็บค่าคะแนนเริ่มต้นไว้ใช้รีเซ็ต

    void Start()
    {
        // ตั้งค่าเริ่มต้นเวลาและคะแนน
        timeLeft = startTime;
        savedTimeLeft = startTime;
        savedScore = score;

        // ตั้งค่าค่าเริ่มต้นของ Slider
        ScoreSlider.maxValue = 100f;
        ScoreSlider.value = score;
        UpdateScoreUI();

        // หา reference ของ PlayerRespawn จาก playerObject
        if (playerObject != null)
        {
            playerRespawn = playerObject.GetComponent<PlayerRespawn>();
        }

        // เริ่มนับเวลารอบแรก
        StartCoroutine(Countdown());
    }

    // ฟังก์ชันแสดงเวลาในรูปแบบ mm:ss
    void UpdateTimeUI()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft % 60f);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // ฟังก์ชันแสดงเปอร์เซ็นต์คะแนน
    void UpdateScoreUI()
    {
        SCORE_PERCENT.text = Mathf.RoundToInt(score) + "%";
        ScoreSlider.value = score;
    }

    // Coroutine: นับเวลาถอยหลัง → เมื่อหมดเวลา → เริ่มลดคะแนน
    IEnumerator Countdown()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0) timeLeft = 0;
            UpdateTimeUI();
            yield return null;
        }

        // เมื่อเวลาหมด → เริ่มลดคะแนน
        StartCoroutine(DropScoreOverTime());
    }

    // Coroutine: ลดคะแนนทีละ 5% → วนไปเรื่อย ๆ → รีเซ็ตเมื่อ < 50%
    IEnumerator DropScoreOverTime()
    {
        while (true)
        {
            score -= 5f;
            if (score < 0) score = 0;
            UpdateScoreUI();

            // ถ้าคะแนนต่ำกว่า 50% → รีเซ็ตและวาร์ปกลับ
            if (score < 50f && playerRespawn != null)
            {
                Debug.Log("🚀 SCORE < 50% → Respawn and Reset!");

                // รีเซ็ตเวลาและคะแนนกลับค่าต้นฉบับ
                timeLeft = savedTimeLeft;
                score = savedScore;
                UpdateTimeUI();
                UpdateScoreUI();

                // เรียกการวาร์ปกลับจุด checkpoint พร้อม fade
                yield return StartCoroutine(playerRespawn.HandleRespawn());

                // รอเล็กน้อยแล้วเริ่มนับเวลารอบใหม่
                yield return new WaitForSeconds(1f);
                StartCoroutine(Countdown());

                yield break; // หยุดการลดคะแนนรอบนี้ (จะถูกเริ่มใหม่โดย Countdown)
            }

            // รอ 1 วินาทีค่อยลดคะแนนอีกครั้ง
            yield return new WaitForSeconds(1f);
        }
    }
}
