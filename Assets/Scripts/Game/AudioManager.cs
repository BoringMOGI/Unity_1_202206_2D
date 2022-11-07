using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] AudioSE sePrefab;
    [SerializeField] AudioClip[] bgmClips;
    [SerializeField] AudioClip[] seClips;

    AudioSource source;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayBGM()
    {
        source.Play();
    }
    public void StopBGM()
    {
        source.Stop();
    }

    public void PlaySE(string seName)
    {
        // 매개변수 seName 이름을 가진 Clip을 찾는다.
        foreach(AudioClip clip in seClips)
        {
            if (clip.name == seName)     // clip의 이름이 seName과 같을 경우.
            {
                AudioSE se = Instantiate(sePrefab, transform);
                se.Setup(clip);
                break;
            }
        }
    }
}
