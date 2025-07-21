using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForce = 10f;
    public bool isDangerousBounce = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // reset Y before bounce
                rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);

                if (isDangerousBounce)
                {
                    Debug.Log("Dangerous Bounce!");
                    // Add visual cue or sound if needed
                }
            }
        }
    }
}
