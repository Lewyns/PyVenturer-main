using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("BGMVolume"))
            PlayerPrefs.SetFloat("BGMVolume", 0.75f);
        if (!PlayerPrefs.HasKey("SFXVolume"))
            PlayerPrefs.SetFloat("SFXVolume", 0.75f);

        LoadVolume();

        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetBGMVolume(float value)
    {
        audioMixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("BGMVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private void LoadVolume()
    {
        float bgm = PlayerPrefs.GetFloat("BGMVolume");
        float sfx = PlayerPrefs.GetFloat("SFXVolume");

        bgmSlider.value = bgm;
        sfxSlider.value = sfx;

        audioMixer.SetFloat("BGMVolume", Mathf.Log10(bgm) * 20);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfx) * 20);
    }
}
