using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public Button resumeButton;
    public Button mainMenuButton;
    public Button settingButton;

    private bool isPaused = false;

    private void Start()
    {
        pauseMenuPanel.SetActive(false);
        resumeButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        settingButton.onClick.AddListener(OpenSetting);

        LockMouse();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void ResumeGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;

        StartCoroutine(LockMouseAfterDelay(0.1f));
    }
    public void PauseGame1()
    {
        isPaused = true;
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame1()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;

        StartCoroutine(LockMouseAfterDelay(0.1f));
    }
    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("Main_Menu");
    }

    private void OpenSetting()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("SETTING_3");
        //Debug.Log("Setting button clicked");
        // เปลี่ยนไปยัง scene ชื่อ Setting หรือแสดง panel ตั้งค่า
        // SceneManager.LoadScene("Setting");
    }

    private IEnumerator LockMouseAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        LockMouse();
    }

    private void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
