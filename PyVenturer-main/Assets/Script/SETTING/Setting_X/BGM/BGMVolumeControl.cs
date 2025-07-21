using UnityEngine;
using UnityEngine.UI;

public class BGMVolumeControl : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Button increaseButton;
    [SerializeField] private Button decreaseButton;
    [SerializeField] private Button muteButton;
    [SerializeField] private AudioSource bgmSource;

    public static float BGMVolume = 1f;

    private bool isIncreasing = false;
    private bool isDecreasing = false;

    private bool isMuted = false;
    private float lastVolume = 1f;

    public float holdSpeed = 0.3f;
    private float holdTimer = 0f;

    private void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("bgmVolume", 1f);
        bgmSlider.value = savedVolume;
        BGMVolume = savedVolume;

        if (bgmSource != null)
            bgmSource.volume = savedVolume;

        bgmSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });

        increaseButton.gameObject.AddComponent<ButtonHold>().Setup(
            () => { IncreaseVolume(); isIncreasing = true; },
            () => isIncreasing = false
        );

        decreaseButton.gameObject.AddComponent<ButtonHold>().Setup(
            () => { DecreaseVolume(); isDecreasing = true; },
            () => isDecreasing = false
        );

        muteButton.onClick.AddListener(ToggleMute);
    }


    private void Update()
    {
        if (isIncreasing)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdSpeed)
            {
                IncreaseVolume();
                holdTimer = 0f;
            }
        }
        else if (isDecreasing)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdSpeed)
            {
                DecreaseVolume();
                holdTimer = 0f;
            }
        }
        else
        {
            holdTimer = 0f;
        }
    }

    private void ChangeVolume()
    {
        BGMVolume = bgmSlider.value;
        if (bgmSource != null)
            bgmSource.volume = bgmSlider.value;
        SaveVolume();
    }

    private void IncreaseVolume()
    {
        bgmSlider.value = Mathf.Clamp(bgmSlider.value + 0.05f, 0f, 1f);
    }

    private void DecreaseVolume()
    {
        bgmSlider.value = Mathf.Clamp(bgmSlider.value - 0.05f, 0f, 1f);
    }

    private void LoadVolume()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("bgmVolume");
        BGMVolume = bgmSlider.value;
        if (bgmSource != null)
            bgmSource.volume = bgmSlider.value;
    }


    private void SaveVolume()
    {
        PlayerPrefs.SetFloat("bgmVolume", bgmSlider.value);
    }

    private void ToggleMute()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            lastVolume = bgmSlider.value;
            bgmSlider.value = 0f;
        }
        else
        {
            bgmSlider.value = lastVolume;
        }

        // เปลี่ยนชื่อปุ่ม Mute / Unmute
        Text btnText = muteButton.GetComponentInChildren<Text>();
        if (btnText != null)
        {
            btnText.text = isMuted ? "Unmute" : "Mute";
        }
    }
}
