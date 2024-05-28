using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

[ExecuteAlways]
public class DisableWhenLoadingAnythingElse : MonoBehaviour
{
#if UNITY_EDITOR
    private void Awake()
    {
        AddAllCallbacks();
        SetEnabledBasedOnLoadedLevels();
    }

    private void AddAllCallbacks()
    {
        EditorSceneManager.sceneLoaded += (_, _) => SetEnabledBasedOnLoadedLevels();
        EditorSceneManager.sceneUnloaded += (_) => SetEnabledBasedOnLoadedLevels();
        EditorSceneManager.sceneOpened += (_, _) => SetEnabledBasedOnLoadedLevels();
        EditorSceneManager.sceneClosed += (_) => SetEnabledBasedOnLoadedLevels();
    }

    private void SetEnabledBasedOnLoadedLevels()
    {
        if (this != null)
            StartCoroutine(CoSetEnabledBasedOnLoadedLevels());
    }
    
    private IEnumerator CoSetEnabledBasedOnLoadedLevels()
    {
        yield return null; // waiting 1 frame is required for some reason, otherwise it doesnt recognise when a scene is deloaded

        if (this == null)
            yield break;

        var State = LevelManager.GetLevelState();
        bool active = State is LevelManager.SceneState.InMainScene;
        foreach (Transform child in transform)
            child.gameObject.SetActive(active);
    }

#else 

    private void Awake()
    {
        Destroy(this);
    }
#endif
}
