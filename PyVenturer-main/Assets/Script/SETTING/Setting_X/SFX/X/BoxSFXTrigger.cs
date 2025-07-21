using UnityEngine;

public class BoxSFXTrigger : MonoBehaviour
{
    public AudioSource sfxAudio;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // ตรวจเฉพาะชนกับผู้เล่น
        {
            sfxAudio.Play();
        }
    }
}
