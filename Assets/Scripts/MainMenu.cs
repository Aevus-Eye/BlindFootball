using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class MainMenu : MonoBehaviour
{
    int? boardScene = null;
    public int boardSceneCount = 8;
    public bool isInGame = false;

    private List<int> level_bag = new List<int>();

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

    public static void Shuffle<T>(IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = UnityEngine.Random.Range(0, n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }

    public void FillLevelBag()
    {
        level_bag.Clear();
        for (int i = 0; i < boardSceneCount; i++)
        {
            level_bag.Add(i + 2);
        }
        Shuffle(level_bag);
    }

    public void LoadRandomScene()
    {
        // load the scene and store it in the BoardScene variable
        if (this.boardScene != null)
            SceneManager.UnloadSceneAsync(this.boardScene.Value);
        if (level_bag.Count == 0)
            FillLevelBag();
        int boardScene = level_bag[0]; 
        level_bag.RemoveAt(0);
        
        // put the scene back in the bag if its the same as the current one
        if (boardScene == this.boardScene) 
        {
            level_bag.Add(boardScene);
            boardScene = level_bag[0];
            level_bag.RemoveAt(0);
        }
        this.boardScene = boardScene;
        SceneManager.LoadSceneAsync(this.boardScene.Value, LoadSceneMode.Additive);
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
