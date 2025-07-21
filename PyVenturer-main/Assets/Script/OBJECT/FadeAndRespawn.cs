using UnityEngine;
using System.Collections;

public class FadeAndRespawn : MonoBehaviour
{
    public float delayBeforeFade = 3f;
    public float fadeDuration = 2f;
    public float respawnDelay = 4f;

    private Material material;
    private Color originalColor;
    private bool playerOnBlock = false;
    private bool isFading = false;
    private float stayTimer = 0f;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        originalColor = material.color;
    }

    void Update()
    {
        if (playerOnBlock && !isFading)
        {
            stayTimer += Time.deltaTime;
            if (stayTimer >= delayBeforeFade)
            {
                StartCoroutine(FadeAndHide());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnBlock = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnBlock = false;
            stayTimer = 0f; // reset ถ้าเดินออกก่อนถึงเวลา
        }
    }

    private IEnumerator FadeAndHide()
    {
        isFading = true;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        gameObject.SetActive(false); // ซ่อนบล็อก

        yield return new WaitForSeconds(respawnDelay);

        // รีเซ็ต
        material.color = originalColor;
        gameObject.SetActive(true);
        stayTimer = 0f;
        isFading = false;
        playerOnBlock = false;
    }
}
