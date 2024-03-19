using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move2d : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (move.sqrMagnitude > 0.01)
        {
            rb.velocity = move * speed;
        }
    }
}
