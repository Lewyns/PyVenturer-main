using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public GameObject activateEffectObject; // มี Particle System อยู่ในตัว
    public AudioSource soundSource;         // ใช้ AudioSource จาก object อื่น

    private bool isActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isActivated) return; // ❌ ถ้าเคยเหยียบแล้ว ข้ามเลย

        if (other.CompareTag("Player"))
        {
            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
            if (respawn != null)
            {
                respawn.UpdateCheckpoint(transform.position);
                Debug.Log("📍 Player touched checkpoint at: " + transform.position);
            }

            if (activateEffectObject != null)
            {
                var particle = activateEffectObject.GetComponent<ParticleSystem>();
                if (particle != null) particle.Play();
            }

            if (soundSource != null)
            {
                soundSource.Play();
            }

            isActivated = true;
        }
    }
}
