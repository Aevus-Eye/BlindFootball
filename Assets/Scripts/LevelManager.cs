#nullable enable

using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.VisualScripting;

// hack to make records work in Unity
namespace System.Runtime.CompilerServices { class IsExternalInit { } }

public class LevelManager : MonoBehaviour
{
    public static class Scenes
    {
        public const string MAIN_MENU = "Menu";
        public const string MAIN_SCENE = "Main";
        public const string LEVEL_PREFIX = "Level_";

        private static List<string>? _allScenesInBuild = null;
        public static List<string> AllScenesInBuild
        {
            get
            {
                _allScenesInBuild ??= Enumerable
                        .Range(0, SceneManager.sceneCountInBuildSettings)
                        .Select(i => System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)))
                        .ToList();
                return _allScenesInBuild;
            }
        }
    }

    // rust like enum for scene state
    public abstract record SceneState()
    {
        public record InMainMenu() : SceneState;
        public record InMainScene() : SceneState;
        public record InLevel(string level) : SceneState;
        public record InMainSceneAndLevel(string level) : SceneState;
        public record Unknown() : SceneState;
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
                Instantiate(Resources.Load("Music"), inst.transform)
                    .GetComponent<AudioSource>()
                    .outputAudioMixerGroup
                    .audioMixer
                    .SetFloat("PitchShift", 1.0f);
                _instance = inst.GetComponent<LevelManager>();
            }
            return _instance;
        }
    }

    string? currentLoadedLevel = null;
    private RandomFeelingBag<string> levelBag = null!;

    public static bool IsLevel(string sceneName) => sceneName.StartsWith(Scenes.LEVEL_PREFIX);

    /// gets the current state of the game. see SceneState for possible states
    public static SceneState GetLevelState()
    {
        List<Scene> scenes = new();
        for (int i = 0; i < SceneManager.sceneCount; i++)
            scenes.Add(SceneManager.GetSceneAt(i));

        scenes.Sort((a, b) => a.buildIndex - b.buildIndex);
        List<string> sceneNames = scenes.Select(s => s.name).ToList();

        return sceneNames.Count switch
        {
            1 => sceneNames[0] switch
            {
                Scenes.MAIN_MENU => new SceneState.InMainMenu(),
                Scenes.MAIN_SCENE => new SceneState.InMainScene(),
                string s when IsLevel(s) => new SceneState.InLevel(s),
                _ => new SceneState.Unknown()
            },
            2 when sceneNames[0] == Scenes.MAIN_SCENE && IsLevel(sceneNames[1])
                => new SceneState.InMainSceneAndLevel(sceneNames[1]),
            _ => new SceneState.Unknown()
        };
    }

    [RuntimeInitializeOnLoadMethod]
    static void Startup()
    {
        print(GetLevelState());
        switch (GetLevelState())
        {
            case SceneState.InMainMenu _:
                // do nothing
                break;
            case SceneState.InMainSceneAndLevel(string level):
                Instance.currentLoadedLevel = level;
                break;
            case SceneState.InMainScene _:
                Instance.LoadNextLevel();
                break;
            case SceneState.InLevel(string level):
                SceneManager.LoadSceneAsync(Scenes.MAIN_SCENE, LoadSceneMode.Additive);
                Instance.currentLoadedLevel = level;
                break;

            case SceneState.Unknown _:
            default:
                Instance.PlayGame();
                break;
        }
    }

    private void Awake() => levelBag = new(3, Scenes.AllScenesInBuild.Where(s => IsLevel(s)).ToList());

    // these StartCoroutine wrappers are neccessary because this GO needs to own the coroutine, 
    // otherwise it gets killed when the scene changes if StartCoroutine is called from a different GO
    public void PlayGame() => StartCoroutine(CoPlayGame());
    public IEnumerator CoPlayGame()
    {
        yield return SceneManager.LoadSceneAsync(1);
        yield return CoLoadNextLevel();
    }

    public void LoadNextLevel() => StartCoroutine(CoLoadNextLevel());
    public IEnumerator CoLoadNextLevel()
    {
        string? tmpCurrentLoadedLevel = currentLoadedLevel;
        string nextLevel = levelBag.GetNextItem();
        currentLoadedLevel = nextLevel;

        yield return SceneManager.LoadSceneAsync(nextLevel, LoadSceneMode.Additive);
        if (tmpCurrentLoadedLevel != null)
            yield return SceneManager.UnloadSceneAsync(tmpCurrentLoadedLevel);
    }

    public void QuitGame() => Application.Quit();

    public void ReturnMenu()
    {
        currentLoadedLevel = null;
        SceneManager.LoadSceneAsync(0);
    }
}
