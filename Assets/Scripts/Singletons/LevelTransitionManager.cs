using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEditor;

#nullable enable

public class LevelTransitionManager : SingletonBaseMono<LevelTransitionManager>
{
    public enum TransitionDirection
    {
        In,
        Out
    }

    public float inDistance = 5;
    public float outDistance = 5;
    public float inLerpTime = 1;
    public float inLerpSpeed = 0.01f;
    public float outLerpTime = 1;
    public float outLerpSpeed = 0.01f;

    public LevelTransitionEffect? effect = null;


    public LevelTransitionEffect Initiate(string sceneName, TransitionDirection dir)

    {
        return new LevelTransitionEffect(this, SceneManager
            .GetSceneByName(sceneName)
            .GetRootGameObjects()
            .SelectMany(go => go.GetComponentsInChildren<Transform>())
            .Select(t => t.gameObject)
            .Where(go => go.layer == 8),
            dir);
    }

    // public void MoveOut()
    // {
    //     effect?.TeleportObjectsOut();
    // }

    // public void MoveIn()
    // {
    //     effect?.TeleportObjectsIn();
    // }
}
