using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        print("Play Game");
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        print("Quit Game");
        Application.Quit();
    }

    public void ReturnMenu()
    {
        print("Return Menu");
        SceneManager.LoadSceneAsync(0);
    }
}
