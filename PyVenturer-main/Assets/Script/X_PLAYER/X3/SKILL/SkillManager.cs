using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public GameObject skillPanel;
    public Button dashButton;
    public Button doubleJumpButton;

    public PlayerMovement_X3 player;
    public ScoreTimerManager scoreTimerManager; // ✅ อ้างถึง ScoreTimerManager

    void Start()
    {
        skillPanel.SetActive(false);

        dashButton.onClick.AddListener(() => ChooseSkill("dash"));
        doubleJumpButton.onClick.AddListener(() => ChooseSkill("doublejump"));
    }

    public void ShowSkillPanel()
    {
        skillPanel.SetActive(true);
        Time.timeScale = 0f; // ⏸️ หยุดเวลาเกม
    }

    void ChooseSkill(string skill)
    {
        if (skill == "dash") player.hasDash = true;
        if (skill == "doublejump") player.hasDoubleJump = true;

        skillPanel.SetActive(false);

        // ✅ เรียกให้ ScoreTimerManager กลับไปนับเวลา
        if (scoreTimerManager != null)
            scoreTimerManager.ResumeAfterSkill();
    }
}
