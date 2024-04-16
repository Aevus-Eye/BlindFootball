using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegisterGoal : MonoBehaviour
{
    public TextMeshPro scoreText;
    int goals = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Goal!");
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.gameObject.transform.position = new Vector3(0, 0, other.gameObject.transform.position.z);
            goals++;
            scoreText.text = goals.ToString();
        }
    }
}
