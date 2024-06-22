using UnityEngine;

public class ResetAudio : MonoBehaviour
{
    void Awake()
    {
        GetComponent<AudioSource>()
            .outputAudioMixerGroup
            .audioMixer
            .SetFloat("PitchShift", 1.0f);
    }
}
