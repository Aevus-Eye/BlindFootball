using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPanner : MonoBehaviour
{
    AudioSource ballHitSound;
    private AudioSource bgMusic;
    public float radiusX = 10;
    public float radiusY = 10;
    public float extraGameRadius = 2;

    public AnimationCurve pitchCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve panCurve = AnimationCurve.EaseInOut(0, -1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        ballHitSound = GetComponent<AudioSource>();
        ballHitSound.Play();
        bgMusic = LevelManager.Instance.GetComponentInChildren<AudioSource>();
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2, bool clamp = false)
    {
        if (clamp)
            value = Mathf.Clamp(value, from1, to1);
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    void Update()
    {
        float pan = Remap(transform.position.x, -radiusX, radiusX, 0, 1);
        float pitch = Remap(transform.position.y, -radiusY, radiusY, 0, 1);
        pan = panCurve.Evaluate(pan);
        pitch = pitchCurve.Evaluate(pitch);

        ballHitSound.panStereo = pan;
        bgMusic.panStereo = pan;
        ballHitSound.outputAudioMixerGroup.audioMixer.SetFloat("PitchShift", pitch);

        if (transform.position.x > radiusX + extraGameRadius || transform.position.x < -radiusX - extraGameRadius ||
            transform.position.y > radiusY + extraGameRadius || transform.position.y < -radiusY - extraGameRadius)
        {
            transform.position = new Vector3(0, 0, transform.position.z);
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        ballHitSound.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(0, 0, transform.position.z), new Vector3(radiusX * 2, radiusY * 2, 0));
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(0, 0, transform.position.z), new Vector3((radiusX + extraGameRadius) * 2, (radiusY + extraGameRadius) * 2, 0));
    }
}
