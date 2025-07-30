using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Audio;

public class SFXVolumeControl : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button increaseButton;
    [SerializeField] private Button decreaseButton;
    [SerializeField] private Button muteButton;
    [SerializeField] private List<AudioSource> sfxSources;

    public static float SFXVolume = 1f;

    private const string SFX_PREF_KEY = "sfxVolume";

    private bool isMuted = false;
    private float lastVolume = 1f;

    private void Start()
    {
        if (!PlayerPrefs.HasKey(SFX_PREF_KEY))
        {
            PlayerPrefs.SetFloat(SFX_PREF_KEY, 1f);
        }

        LoadVolume();

        sfxSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
        increaseButton.onClick.AddListener(IncreaseVolume);
        decreaseButton.onClick.AddListener(DecreaseVolume);
        muteButton.onClick.AddListener(ToggleMute);
        UpdateMuteButtonLabel();
    }

    private void ChangeVolume()
    {
        SFXVolume = sfxSlider.value;
        float dB = Mathf.Log10(SFXVolume <= 0.0001f ? 0.0001f : SFXVolume) * 20f;
        mixer.SetFloat("SFXVolume", dB);
        SaveVolume();
    }


    private void IncreaseVolume()
    {
        sfxSlider.value = Mathf.Clamp(sfxSlider.value + 0.1f, 0f, 1f);
    }

    private void DecreaseVolume()
    {
        sfxSlider.value = Mathf.Clamp(sfxSlider.value - 0.1f, 0f, 1f);
    }

    private void LoadVolume()
    {
        SFXVolume = PlayerPrefs.GetFloat(SFX_PREF_KEY, 1f);
        sfxSlider.value = SFXVolume;
        ApplyVolumeToAllSources(SFXVolume);
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetFloat(SFX_PREF_KEY, sfxSlider.value);
    }

    private void ApplyVolumeToAllSources(float volume)
    {
        foreach (var source in sfxSources)
        {
            if (source != null)
                source.volume = volume;
        }
    }

    private void ToggleMute()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            lastVolume = sfxSlider.value;
            sfxSlider.value = 0f;
        }
        else
        {
            sfxSlider.value = lastVolume;
        }

        ApplyVolumeToAllSources(sfxSlider.value);
        UpdateMuteButtonLabel();
    }

    private void UpdateMuteButtonLabel()
    {
        Text label = muteButton.GetComponentInChildren<Text>();
        if (label != null)
        {
            label.text = isMuted ? "Unmute" : "Mute";
        }
    }
}
