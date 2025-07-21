using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ใช้สำหรับการจัดการ Scene

public class QuitGameButton : MonoBehaviour
{
    public void QuitGame()
    {
        // หากอยู่ใน Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // หากอยู่ในเกมที่ Build แล้ว
        Application.Quit();
#endif
    }
}