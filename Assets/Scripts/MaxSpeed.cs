using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxSpeed : MonoBehaviour
{
    public float maxSpeed;
    // unity is dumb. it gives a warnig that rigidbody hides an inherited member. 
    // but if you declare it as new then it gives a warning that new is redundant since it doesnt hide anything...
    private new Rigidbody2D rigidbody;

    void Start() => rigidbody = GetComponent<Rigidbody2D>();

    void FixedUpdate() => rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, maxSpeed);
}
