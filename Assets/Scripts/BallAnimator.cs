using UnityEngine;

[ExecuteInEditMode]
public class BallAnimator : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    public Material wall_material;

    void Start() => animator = GetComponent<Animator>();

    void Update() => wall_material.SetVector("_ball_pos", transform.position);

    void OnCollisionEnter2D(Collision2D collision) => OnCollisionStay2D(collision);
    void OnCollisionStay2D(Collision2D collision) {
        if (collision.collider.CompareTag("Player")) 
            animator.SetTrigger("hit");
    }   

}
