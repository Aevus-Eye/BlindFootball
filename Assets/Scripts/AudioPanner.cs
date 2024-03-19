using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPanner : MonoBehaviour
{
    AudioSource audioSource;
    public float radius_x = 10;
    public float radius_y = 10;

    public AnimationCurve pitch_curve = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve pan_curve = AnimationCurve.EaseInOut(0, -1, 1, 1);

    // public float max_pitch = 3;
    // public float min_pitch = 0.5f;
    public float max_pan = 1;
    public float min_pan = -1;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2, bool clamp = false)
    {
        if (clamp)
            value = Mathf.Clamp(value, from1, to1);
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    // static float PitchMap01(float value)
    // {
    //     return value * value + value / 2 + 0.5f;
    // }

    // Update is called once per frame
    void Update()
    {
        float pan = Remap(transform.position.x, -radius_x, radius_x, 0, 1);
        float pitch = Remap(transform.position.y, -radius_y, radius_y, 0, 1);
        pan = pan_curve.Evaluate(pan);
        pitch = pitch_curve.Evaluate(pitch);

        // audioSource.pitch = pitch;
        audioSource.panStereo = pan;
        audioSource.outputAudioMixerGroup.audioMixer.SetFloat("PitchShift", pitch);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(0, 0, transform.position.z), new Vector3(radius_x * 2, radius_y * 2, 0));
    }
}