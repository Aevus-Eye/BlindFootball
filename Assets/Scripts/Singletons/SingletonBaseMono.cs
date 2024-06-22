#nullable enable

using UnityEngine;

public class SingletonBaseMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T? _instance = default;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = SingletonManager.singletonGO.GetComponentInChildren<T>();
                if (_instance == null)
                    Debug.LogError($"Singleton of type {typeof(T)} not found on Singleton Prefab");
            }
            return _instance!; // this is safe because if it is null all hell breaks loose, so we would know about it
        }
    }
}