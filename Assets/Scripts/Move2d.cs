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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        // Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // rb.velocity = move * speed;
    }

    // 'Move' input action has been triggered.
    public void OnMove(InputValue value)
    {
        Vector2 move = value.Get<Vector2>();
        rb.velocity = move * speed;
    }
}
