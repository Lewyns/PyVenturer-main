using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 currentCheckpoint;
    public ScreenFader fader; // ลากตัว Script ที่มี FadeOverlay เข้า Inspector

    private void Start()
    {
        currentCheckpoint = transform.position;
    }

    public void UpdateCheckpoint(Vector3 newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallZone"))
        {
            StartCoroutine(HandleRespawn());
        }
    }

    public IEnumerator HandleRespawn()
    {
        yield return StartCoroutine(fader.FadeOut());

        GetComponent<CharacterController>().enabled = false;
        transform.position = currentCheckpoint;
        GetComponent<CharacterController>().enabled = true;

        yield return StartCoroutine(fader.FadeIn());
    }
}
