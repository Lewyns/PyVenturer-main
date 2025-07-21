using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToPreviousScene : MonoBehaviour
{
    private string previousScene;

    void Start()
    {
        if (PlayerPrefs.HasKey("previousScene"))
        {
            previousScene = PlayerPrefs.GetString("previousScene");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }

    public void GoBack()
    {
        if (!string.IsNullOrEmpty(previousScene))
        {
            SceneManager.LoadScene(previousScene);
        }
    }

    public static void SetPreviousScene(string sceneName)
    {
        PlayerPrefs.SetString("previousScene", sceneName);
    }
}
