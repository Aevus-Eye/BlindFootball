using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxSpeed : MonoBehaviour
{
    public float maxSpeed;
    private new Rigidbody2D rigidbody;

    void Start() => rigidbody = GetComponent<Rigidbody2D>();

    void FixedUpdate() => rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxSpeed);
}
