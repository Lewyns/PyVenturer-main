using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneForBack : MonoBehaviour
{
    // เก็บชื่อซีนก่อนหน้าไว้
    public static string previousScene;

    // เรียกใช้ก่อนจะโหลดซีนใหม่ เช่นตอนเปลี่ยนจาก Main_Menu -> Setting
    public static void SetPreviousScene(string sceneName)
    {
        previousScene = sceneName;
    }

    // เรียกจากปุ่ม BACK
    public void GoBack()
    {
        if (!string.IsNullOrEmpty(previousScene))
        {
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.LogWarning("ไม่มีซีนก่อนหน้าให้กลับไป!");
        }
    }
}
