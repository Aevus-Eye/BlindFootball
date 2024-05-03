using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegisterGoal : MonoBehaviour
{
    public TextMeshPro scoreText;
    int goals = 0;
    MainMenu mainMenu;
    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
        mainMenu = FindObjectOfType<MainMenu>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Goal!");
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.gameObject.transform.position = new Vector3(0, 0, other.gameObject.transform.position.z);
            other.gameObject.GetComponent<BallAnimator>().animator.SetTrigger("reset");
            goals++;
            UpdateScoreText();
            mainMenu.LoadRandomScene();
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"{goals}";
    }
}
