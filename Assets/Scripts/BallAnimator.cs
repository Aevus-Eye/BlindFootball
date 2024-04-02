using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAnimator : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        // Überprüfen, ob das kollidierende Objekt ein Spieler ist
        if (collision.collider.tag == "Player") {
            // Trigger 'hit' auslösen, um die hitAnimation zu starten
            animator.SetTrigger("hit");
            print("hit Ball");
        }
    }   
}
