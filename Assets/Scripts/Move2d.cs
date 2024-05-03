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
    // private string player_touch_id = "";
    public int player_touch_id = 0;
    private string player_on_bg = "";
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
            LayerMask layerMask = LayerMask.GetMask("BG");
            if (Physics.Raycast(ray, out RaycastHit hit, layerMask: layerMask, maxDistance: 1000f))
            {
                if (player_on_bg == ""){
                    player_on_bg = hit.collider.gameObject.name;
                    var target_touch1 = hit.point;
                    // target_touch1.z = transform.position.z;
                    rb.position = target_touch1;
                }
// #if !UNITY_EDITOR
//                 if (hit.collider.gameObject.name != player_on_bg)
//                     return;
// #endif
                var target_touch = hit.point;
                // target_touch.z = transform.position.z;
                rb.MovePosition(target_touch);
                return;
            }
        }

        if (player_on_bg != "")
        {
            player_on_bg = "";
            rb.position = new Vector2(10, 10);
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
