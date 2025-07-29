using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 respawnPoint;

    [Header("ตำแหน่งเริ่มต้นเมื่อ Reset")]
    public Vector3 initialPosition = new Vector3(0f, 0f, 0f);

    void Start()
    {
        // 🔁 โหลดจาก PlayerPrefs ถ้ามี
        float x = PlayerPrefs.GetFloat("RespawnX", initialPosition.x);
        float y = PlayerPrefs.GetFloat("RespawnY", initialPosition.y);
        float z = PlayerPrefs.GetFloat("RespawnZ", initialPosition.z);
        respawnPoint = new Vector3(x, y, z);

        // ✅ วาร์ปไปยังจุดโหลด
        transform.position = respawnPoint;
        Debug.Log("🎮 Loaded position: " + respawnPoint);
    }

    public void UpdateCheckpoint(Vector3 newCheckpoint)
    {
        respawnPoint = newCheckpoint;

        // 💾 เซฟตำแหน่งล่าสุด
        PlayerPrefs.SetFloat("RespawnX", newCheckpoint.x);
        PlayerPrefs.SetFloat("RespawnY", newCheckpoint.y);
        PlayerPrefs.SetFloat("RespawnZ", newCheckpoint.z);
        PlayerPrefs.Save();

        Debug.Log("💾 Checkpoint saved: " + newCheckpoint);
    }

    public void HandleRespawn()
    {
        transform.position = respawnPoint;
        Debug.Log("🔁 Player respawned at: " + respawnPoint);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetProgress();
        }
    }

    public void ResetProgress()
    {
        // 🗑 ลบข้อมูลเซฟ
        PlayerPrefs.DeleteKey("RespawnX");
        PlayerPrefs.DeleteKey("RespawnY");
        PlayerPrefs.DeleteKey("RespawnZ");
        PlayerPrefs.Save();

        // 🔁 กลับไปยังจุดเริ่มต้น
        transform.position = initialPosition;
        respawnPoint = initialPosition;

        Debug.Log("🔄 Progress reset to: " + initialPosition);
    }

    // ✅ ถ้าตกลง FallZone (Tag == "FallZone") ให้รีกลับ
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FallZone"))
        {
            Debug.Log("☠️ ตก FallZone → Respawn ทันที!");
            HandleRespawn();
        }
    }

    // ✅ ถ้าตกลง Trigger ก็รองรับด้วย (กรณีใช้ Trigger แทน Collider)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FallZone"))
        {
            Debug.Log("☠️ เข้าสู่ FallZone (Trigger) → Respawn!");
            HandleRespawn();
        }
    }
}
