using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneOnClick : MonoBehaviour
{
    public Image fadeImage;
    public CanvasGroup uiGroup;
    public float fadeDuration = 0.3f;

    public static string previousScene;

    // ไปซีนใหม่ + บันทึกซีนก่อนหน้า
    public void ChangeScene(string sceneName)
    {
        previousScene = SceneManager.GetActiveScene().name;
        StartCoroutine(FadeAndLoad(sceneName));
    }

    // ปุ่ม BACK
    public void GoBack()
    {
        if (!string.IsNullOrEmpty(previousScene))
        {
            StartCoroutine(FadeAndLoad(previousScene));
        }
        else
        {
            Debug.LogWarning("ไม่มีซีนก่อนหน้าให้กลับไป!");
        }
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(t / fadeDuration);

            if (uiGroup != null)
                uiGroup.alpha = 1f - alpha;

            if (fadeImage != null)
            {
                c.a = alpha;
                fadeImage.color = c;
            }

            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
