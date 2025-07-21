using UnityEngine;

public class PlaySFXOnCollision : MonoBehaviour
{
    public AudioSource sfxAudio;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            sfxAudio.Play();
        }
    }
}
