using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public float autoDestroyAudioSourcesTimeInterval = 5f;
    private float currentTimeInterval = 0f;

    private void Update()
    {
        if(currentTimeInterval >= autoDestroyAudioSourcesTimeInterval)
        {
            currentTimeInterval = 0f;
            AudioSource[] audioSources = GetComponents<AudioSource>();
            foreach(AudioSource audioSource in audioSources)
            {
                if(!audioSource.isPlaying)
                {
                    Destroy(audioSource);
                }
            }
        }
        else
        {
            currentTimeInterval += Time.deltaTime;
        }
    }
}
