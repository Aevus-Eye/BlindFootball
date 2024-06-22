using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FollowPath : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float onPathSpeed = 1.0f;
    public float lerpSpeed = 0.1f;

    private float distance;
    // private Vector3[] positions;
    private int index = 0;

    private Rigidbody2D rb;

    void Start()
    {
        // positions = new Vector3[lineRenderer.positionCount];
        // lineRenderer.GetPositions(positions);
        // for (int i = 0; i < positions.Length; i++)
        // {
        //     positions[i] = lineRenderer.transform.TransformPoint(positions[i]);
        // }
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        distance += onPathSpeed * Time.deltaTime;
        var posCount = lineRenderer.positionCount;
        Vector3 curPoint = lineRenderer.transform.TransformPoint(lineRenderer.GetPosition(index));
        Vector3 nextPoint = lineRenderer.transform.TransformPoint(lineRenderer.GetPosition((index + 1) % posCount));
        if (distance > Vector3.Distance(curPoint, nextPoint))
            distance = Vector3.Distance(curPoint, nextPoint);

        Vector2 pos = Vector3.Lerp(curPoint, nextPoint, distance / Vector3.Distance(curPoint, nextPoint));
        if (distance >= Vector3.Distance(curPoint, nextPoint))
        {
            distance = 0;
            index = (index + 1) % posCount;
        }

        Vector2 currentPos = transform.position;
        rb.MovePosition(Vector2.Lerp(currentPos, pos, lerpSpeed));
    }
}
