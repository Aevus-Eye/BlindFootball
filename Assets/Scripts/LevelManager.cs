using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using Unity.VisualScripting;

// hack to make records work in Unity
namespace System.Runtime.CompilerServices { class IsExternalInit { } }

public class LevelManager : MonoBehaviour
{
    private static LevelManager? _instance = null;
    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var inst = new GameObject("LevelManager", typeof(LevelManager));
                DontDestroyOnLoad(inst);
                _instance = inst.GetComponent<LevelManager>();
            }
            return _instance;
        }
    }

    // rust like enum for scene state
    public abstract record SceneState()
    {
        public record InMainMenu() : SceneState;
        public record InMainScene() : SceneState;
        public record InLevel(int level) : SceneState;
        public record InMainSceneAndLevel(int level) : SceneState;
        public record Unknown() : SceneState;
    }

    public static SceneState GetLevelState()
    {
        List<int> scenes = new();
        for (int i = 0; i < SceneManager.sceneCount; i++)
            scenes.Add(SceneManager.GetSceneAt(i).buildIndex);
        scenes.Sort();

        if (scenes.Count == 1)
        {
            if (scenes[0] == 0)
                return new SceneState.InMainMenu();
            else if (scenes[0] == 1)
                return new SceneState.InMainScene();
            else
                return new SceneState.InLevel(scenes[0]);
        }
        else if (scenes.Count == 2 && scenes[0] == 1)
            return new SceneState.InMainSceneAndLevel(scenes[1]);
        else
            return new SceneState.Unknown();
    }

    [RuntimeInitializeOnLoadMethod]
    static void Startup()
    {
        switch (GetLevelState())
        {
            case SceneState.InMainMenu _:
                // do nothing
                break;
            case SceneState.InMainSceneAndLevel(int level):
                Instance.currentLoadedLevel = level;
                break;
            case SceneState.InMainScene _:
                Instance.LoadNextLevel();
                break;
            case SceneState.InLevel(int level):
                SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
                Instance.currentLoadedLevel = level;
                break;

            case SceneState.Unknown _:
            default:
                Instance.PlayGame();
                break;
        }
    }

    const int LEVEL_COUNT = 8;
    int? currentLoadedLevel = null;

    private readonly List<int> levelBag = new();

    // necessary because this GO needs to own the coroutine, otherwise it gets killed when the scene changes
    public void PlayGame() => StartCoroutine(CoPlayGame());
    public IEnumerator CoPlayGame()
    {
        yield return SceneManager.LoadSceneAsync(1);
        yield return CoLoadNextLevel();
    }

    private void FillLevelBag()
    {
        levelBag.Clear();
        for (int i = 0; i < LEVEL_COUNT; i++)
            levelBag.Add(i + 2);
        levelBag.Shuffle();
    }

    private int GetNextLevel(int? currentLevel)
    {
        if (levelBag.Count == 0)
            FillLevelBag();
        int nextLevel = levelBag[0];
        levelBag.RemoveAt(0);

        // put the scene back in the bag if its the same as the current one
        if (currentLevel == nextLevel)
        {
            levelBag.Add(nextLevel);
            nextLevel = levelBag[0];
            levelBag.RemoveAt(0);
        }
        return nextLevel;
    }

    public void LoadNextLevel() => StartCoroutine(CoLoadNextLevel());

    public IEnumerator CoLoadNextLevel()
    {
        int? tmpCurrentLoadedLevel = currentLoadedLevel;
        int nextLevel = GetNextLevel(tmpCurrentLoadedLevel);
        currentLoadedLevel = nextLevel; 

        yield return SceneManager.LoadSceneAsync(nextLevel, LoadSceneMode.Additive);
        if (tmpCurrentLoadedLevel != null)
            yield return SceneManager.UnloadSceneAsync(tmpCurrentLoadedLevel.Value);
    }

    public void QuitGame() => Application.Quit();

    public void ReturnMenu() => SceneManager.LoadSceneAsync(0);
}
