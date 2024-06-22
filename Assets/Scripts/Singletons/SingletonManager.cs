#nullable enable

using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

class SingletonManager : SingletonBaseMono<SingletonManager>
{
    public static GameObject singletonGO = null!;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void LoadSingletonPrefab()
    {
        var singletonPrefab = Resources.Load<GameObject>("Singleton");
        if (singletonPrefab == null)
        {
            Debug.LogError("SingletonManager prefab not found in Resources folder");
            return;
        }
        singletonGO = Instantiate(singletonPrefab);
        DontDestroyOnLoad(singletonGO);
    }
}