using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move2d : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerInput input;
    public float speed = 1;

    private Vector2 movement_input;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
    }

    void FixedUpdate()
    {
        rb.velocity = movement_input * speed * Time.fixedDeltaTime;
    }

    // 'Move' input action has been triggered.
    public void OnMove(InputValue value)
    {
        movement_input = value.Get<Vector2>();
        // Debug.Log(movement_input);
    }
}
