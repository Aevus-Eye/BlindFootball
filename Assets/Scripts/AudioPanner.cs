using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPanner : MonoBehaviour
{
    AudioSource audioSource;
    public float radius_x = 10;
    public float radius_y = 10;
    public float extra_radius_for_remove = 2;

    public AnimationCurve pitch_curve = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve pan_curve = AnimationCurve.EaseInOut(0, -1, 1, 1);

    public float max_pan = 1;
    public float min_pan = -1;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2, bool clamp = false)
    {
        if (clamp)
            value = Mathf.Clamp(value, from1, to1);
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    void Update()
    {
        float pan = Remap(transform.position.x, -radius_x, radius_x, 0, 1);
        float pitch = Remap(transform.position.y, -radius_y, radius_y, 0, 1);
        pan = pan_curve.Evaluate(pan);
        pitch = pitch_curve.Evaluate(pitch);

        audioSource.panStereo = pan;
        audioSource.outputAudioMixerGroup.audioMixer.SetFloat("PitchShift", pitch);

        if (transform.position.x > radius_x + extra_radius_for_remove || transform.position.x < -radius_x - extra_radius_for_remove ||
            transform.position.y > radius_y + extra_radius_for_remove || transform.position.y < -radius_y - extra_radius_for_remove)
        {
            transform.position = new Vector3(0, 0, transform.position.z);
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        audioSource.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(0, 0, transform.position.z), new Vector3(radius_x * 2, radius_y * 2, 0));
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(0, 0, transform.position.z), new Vector3((radius_x + extra_radius_for_remove) * 2, (radius_y + extra_radius_for_remove) * 2, 0));
    }
}
