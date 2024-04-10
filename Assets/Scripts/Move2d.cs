using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move2d : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerInput input;
    
    Animator animator;
    public float speed = 1;

    private Vector2 movement_input;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        rb.velocity = movement_input * speed * Time.fixedDeltaTime;

        if ( movement_input == Vector2.zero) {
            animator.SetBool("Move", false);
        }
    }

    // 'Move' input action has been triggered.
    public void OnMove(InputValue value)
    {
        movement_input = value.Get<Vector2>();
        animator.SetBool("Move", true);
        // Debug.Log(movement_input);
    }
}
