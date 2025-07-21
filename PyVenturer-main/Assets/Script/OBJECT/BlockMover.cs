using UnityEngine;

public class BlockMover : MonoBehaviour
{
    private float moveDistance;
    private float moveSpeed;
    private Vector3 startPos;
    private float timeOffset;

    void Start()
    {
        // สุ่มระยะ, ความเร็ว และเวลา offset ให้ไม่เหมือนกันทุกกล่อง
        moveDistance = Random.Range(1f, 5f);
        moveSpeed = Random.Range(1f, 3f);
        timeOffset = Random.Range(0f, 2f);
        startPos = transform.position;
    }

    void Update()
    {
        float offset = Mathf.Sin((Time.time + timeOffset) * moveSpeed) * moveDistance;
        transform.position = startPos + new Vector3(offset, 0, 0);
    }
}
