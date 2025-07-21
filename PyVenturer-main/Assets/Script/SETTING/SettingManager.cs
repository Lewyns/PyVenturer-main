using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;

public class SettingManager : MonoBehaviour
{
    public GameObject settingPanel;
    public Volume blurVolume;

    [Header("Extra Cameras")]
    public Camera uiCamera; // กล้องที่ใช้ render SettingPanel (UICamera)

    [Header("Transition Settings")]
    public float transitionDuration = 0.5f;

    private Coroutine blurRoutine;

    void Start()
    {
        blurVolume.weight = 0f;
        settingPanel.SetActive(false);
        uiCamera.enabled = false; // ซ่อนไว้ก่อน
    }

    public void OpenSetting()
    {
        settingPanel.SetActive(true);
        uiCamera.enabled = true;

        if (blurRoutine != null)
            StopCoroutine(blurRoutine);
        blurRoutine = StartCoroutine(SmoothBlur(blurVolume.weight, 1f));
    }

    public void CloseSetting()
    {
        if (blurRoutine != null)
            StopCoroutine(blurRoutine);
        blurRoutine = StartCoroutine(SmoothBlur(blurVolume.weight, 0f));

        StartCoroutine(DelayHidePanel(transitionDuration));
    }

    IEnumerator SmoothBlur(float from, float to)
    {
        float elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);
            blurVolume.weight = Mathf.Lerp(from, to, t);
            yield return null;
        }

        blurVolume.weight = to;
    }

    IEnumerator DelayHidePanel(float delay)
    {
        yield return new WaitForSeconds(delay);
        settingPanel.SetActive(false);
        uiCamera.enabled = false;
    }
}
