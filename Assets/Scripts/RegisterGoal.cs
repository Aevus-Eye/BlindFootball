using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RegisterGoal : MonoBehaviour
{
    private static int totalGoals = 0;

    public TextMeshPro scoreText;
    int goals = 0;
    LevelManager mainMenu;

    void Start()
    {
        UpdateScoreText();
        mainMenu = FindObjectOfType<LevelManager>();
        totalGoals = 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.gameObject.transform.position = new Vector3(0, 0, other.gameObject.transform.position.z);
            other.gameObject.GetComponent<BallAnimator>().animator.SetTrigger("reset");
            goals++;
            totalGoals++;
            UpdateScoreText();

            // only change the level every third goal
            if (totalGoals % 3 == 0)
                mainMenu.LoadNextLevel();
        }
    }

    private void UpdateScoreText() => scoreText.text = $"{goals}";
}
