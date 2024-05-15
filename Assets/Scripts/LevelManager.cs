#nullable enable
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

// hack to make records work in Unity
namespace System.Runtime.CompilerServices { class IsExternalInit { } }

public class LevelManager : MonoBehaviour
{
    public static class SceneID
    {
        public const int MAIN_MENU = 0;
        public const int MAIN_SCENE = 1;
        public const int LEVEL_START = 2;
        public const int LEVEL_COUNT = 8;
    }

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

    int? currentLoadedLevel = null;
    readonly LevelBag levelBag = new(3, Enumerable.Range(SceneID.LEVEL_START, SceneID.LEVEL_COUNT).ToList());

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

        return scenes.Count switch
        {
            1 => scenes[0] switch
            {
                SceneID.MAIN_MENU => new SceneState.InMainMenu(),
                SceneID.MAIN_SCENE => new SceneState.InMainScene(),
                int l => new SceneState.InLevel(l),
            },
            2 when scenes[0] == SceneID.MAIN_SCENE => new SceneState.InMainSceneAndLevel(scenes[1]),
            _ => new SceneState.Unknown()
        };
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

    // necessary because this GO needs to own the coroutine, otherwise it gets killed when the scene changes
    public void PlayGame() => StartCoroutine(CoPlayGame());
    public IEnumerator CoPlayGame()
    {
        yield return SceneManager.LoadSceneAsync(1);
        yield return CoLoadNextLevel();
    }

    public void LoadNextLevel() => StartCoroutine(CoLoadNextLevel());
    public IEnumerator CoLoadNextLevel()
    {
        int? tmpCurrentLoadedLevel = currentLoadedLevel;
        int nextLevel = levelBag.GetNextLevel();
        currentLoadedLevel = nextLevel;

        yield return SceneManager.LoadSceneAsync(nextLevel, LoadSceneMode.Additive);
        if (tmpCurrentLoadedLevel != null)
            yield return SceneManager.UnloadSceneAsync(tmpCurrentLoadedLevel.Value);
    }

    public void QuitGame() => Application.Quit();

    public void ReturnMenu() => SceneManager.LoadSceneAsync(0);
}
