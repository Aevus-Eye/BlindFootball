using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class LevelTransitionEffect
{
    record SceneObject(GameObject GameObject, Vector3 InPos, Vector3 OutPos);

    readonly List<SceneObject> sceneObjects;
    LevelTransitionManager ltm;

    public Vector3 CalculateOutPos(Vector3 inPos, LevelTransitionManager.TransitionDirection tdir)
    {
        var dir = inPos.To2D().normalized;
        if (dir == Vector2.zero)
            dir = Vector2.down;
        var off = dir * (tdir==LevelTransitionManager.TransitionDirection.Out ? ltm.outDistance : ltm.inDistance);
        return inPos + off.To3D();
    }

    public LevelTransitionEffect(LevelTransitionManager ltm, IEnumerable<GameObject> roots, LevelTransitionManager.TransitionDirection dir)
    {
        this.ltm = ltm;
        sceneObjects = roots.Select(root => new SceneObject(root, root.transform.localPosition,
            CalculateOutPos(root.transform.localPosition, dir))).ToList();
    }

    public void TeleportObjectsIn()
    {
        foreach (var sceneObject in sceneObjects)
            sceneObject.GameObject.transform.localPosition = sceneObject.InPos;
    }

    public IEnumerator MoveObjectsInCoroutine()
    {
        yield return new WaitForFixedUpdate();
        for (float t = 0; t < ltm.inLerpTime; t += Time.deltaTime)
        {
            foreach (var sceneObject in sceneObjects)
            {
                sceneObject.GameObject.transform.localPosition =
                    Vector3.Lerp(sceneObject.GameObject.transform.localPosition, sceneObject.InPos, ltm.inLerpSpeed);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public void TeleportObjectsOut()
    {
        foreach (var sceneObject in sceneObjects)
            sceneObject.GameObject.transform.localPosition = sceneObject.OutPos;
    }

    public IEnumerator MoveObjectsOutCoroutine(Action onCompleted = null)
    {
        yield return new WaitForFixedUpdate();
        for (float t = 0; t < ltm.outLerpTime; t += Time.deltaTime)
        {
            foreach (var sceneObject in sceneObjects)
            {
                sceneObject.GameObject.transform.localPosition =
                    Vector3.Lerp(sceneObject.GameObject.transform.localPosition, sceneObject.OutPos, ltm.outLerpSpeed);
            }
            yield return new WaitForFixedUpdate();
        }
        onCompleted?.Invoke();
    }
}