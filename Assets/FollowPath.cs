using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class FollowPath : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float onPathSpeed = 5.0f;
    public float lerpSpeed = 0.1f;
    private float distance;
    private Vector3[] positions;
    private int index = 0;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = lineRenderer.transform.TransformPoint(positions[i]);
        }
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distance += onPathSpeed * Time.deltaTime;
        if (distance > Vector3.Distance(positions[index], positions[(index + 1) % positions.Length]))
        {
            distance = Vector3.Distance(positions[index], positions[(index + 1) % positions.Length]);
        }
        Vector2 pos = Vector3.Lerp(positions[index], positions[(index + 1) % positions.Length], distance / Vector3.Distance(positions[index], positions[(index + 1) % positions.Length]));
        if (distance >= Vector3.Distance(positions[index], positions[(index + 1) % positions.Length]))
        {
            distance = 0;
            index = (index + 1) % positions.Length;
        }

        Vector2 currentPos = transform.position;
        rb.MovePosition(Vector2.Lerp(currentPos, pos, lerpSpeed));
    }
}
