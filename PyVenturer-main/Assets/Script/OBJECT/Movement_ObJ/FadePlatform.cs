using UnityEngine;
using System.Collections;

public class FadePlatform : MonoBehaviour
{
    public float fadeDuration = 1.5f; // เวลาที่ใช้ในการเลือนหาย
    public float cooldown = 3f;       // เวลาคูลดาวน์ก่อนกลับมา

    private Renderer rend;
    private Collider col;
    private bool isFading = false;
    private Color originalColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();

        // ดึงสีต้นฉบับจาก HDRP material (_BaseColor)
        originalColor = rend.material.GetColor("_BaseColor");
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isFading && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadeAndRespawn());
        }
    }

    IEnumerator FadeAndRespawn()
    {
        isFading = true;
        float elapsed = 0f;

        // เริ่มค่อยๆจาง
        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            rend.material.SetColor("_BaseColor", new Color(originalColor.r, originalColor.g, originalColor.b, alpha));
            elapsed += Time.deltaTime;
            yield return null;
        }

        // หายหมด + ปิด Collider
        rend.material.SetColor("_BaseColor", new Color(originalColor.r, originalColor.g, originalColor.b, 0f));
        col.enabled = false;

        yield return new WaitForSeconds(cooldown);

        // กลับมาปกติ
        rend.material.SetColor("_BaseColor", originalColor);
        col.enabled = true;
        isFading = false;
    }
}
