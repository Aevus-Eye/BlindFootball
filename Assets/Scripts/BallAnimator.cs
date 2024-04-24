using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BallAnimator : MonoBehaviour
{
    Animator animator;
    public GameObject trail;
    public Material wall_material;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        wall_material.SetVector("_ball_pos", transform.position);
    }

    IEnumerator follow_ball()
    {
        trail.transform.position = transform.position;
        yield return null;
    }

    void OnCollisionEnter2D(Collision2D collision) => OnCollisionStay2D(collision);
    void OnCollisionStay2D(Collision2D collision) {
        if (collision.collider.tag == "Player") {
            animator.SetTrigger("hit");

        }
    }   

}
