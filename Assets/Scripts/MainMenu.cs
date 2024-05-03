using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        print("Play Game");
        SceneManager.LoadSceneAsync(1);

        // Chose random level additiv
        // LogLevel 2 - 6
        int level = UnityEngine.Random.Range(2, 8);
        print(level);
        SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
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
