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
        // �Ű����� seName �̸��� ���� Clip�� ã�´�.
        foreach(AudioClip clip in seClips)
        {
            if (clip.name == seName)     // clip�� �̸��� seName�� ���� ���.
            {
                AudioSE se = Instantiate(sePrefab, transform);
                se.Setup(clip);
                break;
            }
        }
    }
}
