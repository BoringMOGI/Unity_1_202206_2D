using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSE : MonoBehaviour
{
    AudioSource source;
    
    public void Setup(AudioClip clip)
    {
        source = GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
    }
    private void Update()
    {
        if (source.isPlaying)
            return;

        Destroy(gameObject);
    }
}
