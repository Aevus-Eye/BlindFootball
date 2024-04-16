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
    public int player_touch_id = 0;
    // public Vector3 velocity;
    // private Vector3 target_touch;

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
        // var dif = target_touch - transform.position;
        // rb.velocity = dif;//* Time.fixedDeltaTime;
        // print(dif);

        if (movement_input == Vector2.zero)
        {
            animator.SetBool("Move", false);
        }
    }

    void Update()
    {
        // velocity = rb.velocity;
        // get touch input and move ball to that position
        if (Input.touchCount > player_touch_id)
        {
            Touch touch = Input.GetTouch(player_touch_id);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var target_touch = hit.point;
                target_touch.z = transform.position.z;
                // velocity = transform.position - target_touch;
                rb.MovePosition(target_touch);
                // transform.position = target_touch;
            }
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
