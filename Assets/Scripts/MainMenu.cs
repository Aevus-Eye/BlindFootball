using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    int? boardScene = null;
    public int boardSceneCount = 6;
    public bool isInGame = false;

    private void Awake() {
        if (isInGame){
            LoadRandomScene();
        }
    }

    public void PlayGame()
    {
        print("Play Game");
        SceneManager.LoadSceneAsync(1);
    }

    public void LoadRandomScene()
    {
        // load the scene and store it in the BoardScene variable
        if (boardScene != null)
            SceneManager.UnloadSceneAsync(boardScene.Value);
        boardScene = UnityEngine.Random.Range(0, boardSceneCount) + 2;
        SceneManager.LoadSceneAsync(boardScene.Value, LoadSceneMode.Additive);
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
