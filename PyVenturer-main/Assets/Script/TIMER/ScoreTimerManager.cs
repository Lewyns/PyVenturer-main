using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ScoreTimerManager : MonoBehaviour
{
    [Header("เวลา")]
    public TMP_Text timeText;
    public float startTime = 150f;
    private float timeLeft;

    [Header("คะแนน")]
    public TMP_Text SCORE_PERCENT;
    public Slider ScoreSlider;
    private float score = 100f;

    [Header("Player & Checkpoint")]
    public GameObject playerObject;
    private PlayerRespawn playerRespawn;

    [Header("Skill Panel")]
    public SkillManager skillManager;

    private int lowScoreCount = 0;
    private int maxLowScoreCount = 5;

    private float savedTimeLeft;
    private float savedScore;

    private bool hasChosenSkill = false; // ✅ แสดงสกิลได้แค่ครั้งเดียว

    void Start()
    {
        timeLeft = startTime;
        savedTimeLeft = startTime;
        savedScore = score;

        ScoreSlider.maxValue = 100f;
        ScoreSlider.value = score;
        UpdateScoreUI();

        if (playerObject != null)
            playerRespawn = playerObject.GetComponent<PlayerRespawn>();

        StartCoroutine(Countdown());
    }

    void UpdateTimeUI()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft % 60f);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void UpdateScoreUI()
    {
        SCORE_PERCENT.text = Mathf.RoundToInt(score) + "%";
        ScoreSlider.value = score;
    }

    IEnumerator Countdown()
    {
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0) timeLeft = 0;
            UpdateTimeUI();
            yield return null;
        }

        StartCoroutine(DropScoreOverTime());
    }

    IEnumerator DropScoreOverTime()
    {
        while (true)
        {
            score -= 5f;
            if (score < 0) score = 0;
            UpdateScoreUI();

            if (score < 50f && playerRespawn != null)
            {
                Debug.Log("🚀 SCORE < 50% → Respawn and Reset!");
                lowScoreCount++;

                if (lowScoreCount >= maxLowScoreCount && !hasChosenSkill)
                {
                    Debug.Log("🛑 ครบ 5 รอบแล้ว → แสดงหน้าเลือก Skill");

                    Time.timeScale = 0f;
                    hasChosenSkill = true;

                    if (skillManager != null)
                        skillManager.ShowSkillPanel();

                    yield break;
                }

                timeLeft = savedTimeLeft;
                score = savedScore;
                UpdateTimeUI();
                UpdateScoreUI();

                playerRespawn.HandleRespawn();              // ✅ เปลี่ยนจาก Coroutine → Call ตรง
                yield return new WaitForSeconds(1f);        // ⏳ Delay
                StartCoroutine(Countdown());
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public void ResumeAfterSkill()
    {
        Debug.Log("⏱️ เริ่มนับเวลาใหม่หลังเลือกสกิล");

        timeLeft = savedTimeLeft;
        UpdateTimeUI();

        Time.timeScale = 1f;
        StartCoroutine(Countdown());
    }
}
