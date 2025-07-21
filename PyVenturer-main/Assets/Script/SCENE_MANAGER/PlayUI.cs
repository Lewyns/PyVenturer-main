using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayUI : MonoBehaviour
{
    public void GoToSetting()
    {
        // เก็บชื่อซีนก่อนหน้าไว้ก่อน
        ChangeSceneForBack.SetPreviousScene(SceneManager.GetActiveScene().name);

        // ค่อยโหลดซีน Setting
        SceneManager.LoadScene("Setting");
    }
}
