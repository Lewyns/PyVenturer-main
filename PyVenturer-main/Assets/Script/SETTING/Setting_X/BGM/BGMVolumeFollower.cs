using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMVolumeFollower : MonoBehaviour
{
    [SerializeField] private string volumeKey = "bgmVolume"; // หรือใช้ "sfxVolume"
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // โหลดค่าระดับเสียงจาก PlayerPrefs
        float volume = PlayerPrefs.GetFloat(volumeKey, 1f);
        audioSource.volume = volume;
    }
}
