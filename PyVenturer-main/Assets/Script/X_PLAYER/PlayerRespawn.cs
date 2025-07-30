using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 respawnPoint;

    [Header("Fade Effect")]
    public CanvasGroup fadeOverlay;  // ลาก Panel ที่มี CanvasGroup ใส่ตรงนี้
    public float fadeDuration = 0.5f;

    void Start()
    {
        respawnPoint = transform.position;
        Debug.Log("🎮 Game started. Initial spawn at: " + respawnPoint);
    }

    public void UpdateCheckpoint(Vector3 newCheckpoint)
    {
        respawnPoint = newCheckpoint;
        Debug.Log("✅ Updated checkpoint to: " + respawnPoint);
    }

    public void HandleRespawn()
    {
        StartCoroutine(FadeAndRespawn());
    }

    private IEnumerator FadeAndRespawn()
    {
        Debug.Log("🕳️ เริ่ม Fade ดำก่อน Respawn");

        // 1. Fade to black
        if (fadeOverlay != null)
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                fadeOverlay.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
                yield return null;
            }
            fadeOverlay.alpha = 1f;
        }

        // 2. Teleport
        var controller = GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
            transform.position = respawnPoint;
            controller.enabled = true;
        }
        else
        {
            transform.position = respawnPoint;
        }

        Debug.Log("🔁 Respawned to: " + respawnPoint);

        // 3. Fade back in
        if (fadeOverlay != null)
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                fadeOverlay.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
                yield return null;
            }
            fadeOverlay.alpha = 0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FallZone"))
        {
            Debug.Log("☠️ [FALLZONE COLLISION] Player ตกลง FallZone (ชนปกติ) → Respawn!");
            HandleRespawn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallZone"))
        {
            Debug.Log("☠️ [FALLZONE TRIGGER] Player เข้า FallZone (Trigger) → Respawn!");
            HandleRespawn();
        }
    }
}
