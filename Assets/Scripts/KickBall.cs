using UnityEngine;

public class KickBall : MonoBehaviour
{
    public float force = 10;
    public float minForce = 10;

    Move2d move2d;

    void Start()
    {
        move2d = GetComponent<Move2d>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ball")
        {
            Vector2 direction = collision.collider.transform.position - transform.position;
            var ball_velocity = move2d.velocity;
            var _force = Mathf.Max(minForce, ball_velocity.magnitude * force) * direction.normalized;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(_force, ForceMode2D.Impulse);
        }
    }
}