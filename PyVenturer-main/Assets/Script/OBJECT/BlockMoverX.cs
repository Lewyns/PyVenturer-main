using UnityEngine;

public class BlockMoverX : MonoBehaviour
{
    public float moveDistance = 3f;
    public float moveSpeed = 2f;
    public bool startRight = true; // ถ้า true เริ่มไปขวา, ถ้า false เริ่มไปซ้าย

    private Vector3 startPos;
    private float directionMultiplier;

    void Start()
    {
        startPos = transform.position;
        directionMultiplier = startRight ? 1f : -1f;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * moveSpeed) * moveDistance * directionMultiplier;
        transform.position = startPos + new Vector3(offset, 0, 0);
    }
}
