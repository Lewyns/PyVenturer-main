using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXVolumeFollower : MonoBehaviour
{
    [SerializeField] private string volumeKey = "sfxVolume"; // หรือใช้ "sfxVolume"
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // โหลดค่าระดับเสียงจาก PlayerPrefs
        float volume = PlayerPrefs.GetFloat(volumeKey, 1f);
        audioSource.volume = volume;
    }
}
