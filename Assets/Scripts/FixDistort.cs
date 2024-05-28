using System.Collections;
using UnityEngine;

public class FixDistort : MonoBehaviour
{
    // this is a unity bug where the render priority is sometimes not set correctly on editor startup or project build
    IEnumerator Start()
    {
        Material material = GetComponent<Renderer>().material;
        int priority = material.renderQueue;
        material.renderQueue = priority - 1;
        yield return null;
        material.renderQueue = priority;
    }
}
