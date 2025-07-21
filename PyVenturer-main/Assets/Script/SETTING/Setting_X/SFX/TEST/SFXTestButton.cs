using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SFXTestButton : MonoBehaviour
{
    [SerializeField] private AudioSource testSFXSource;
    [SerializeField] private string volumeKey = "sfxVolume";

    private void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(PlayTestSFX);
    }

    private void PlayTestSFX()
    {
        if (testSFXSource != null)
        {
            float volume = PlayerPrefs.GetFloat(volumeKey, 1f);
            testSFXSource.volume = volume;
            testSFXSource.Play();
        }
    }
}
