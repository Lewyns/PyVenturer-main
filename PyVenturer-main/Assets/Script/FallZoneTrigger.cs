using UnityEngine;

public class FallZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
            if (respawn != null)
            {
                Debug.Log("☠️ ตก FallZone → รี Spawn!");
                respawn.HandleRespawn();
            }
        }
    }
}
