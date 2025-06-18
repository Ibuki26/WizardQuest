using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [SerializeField] private List<AudioData> audioDataList;
    [SerializeField] private AudioSource bgm;
    private static float SESoundSize = 0.5f;
    private static float BGMSoundSize = 0.2f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = SESoundSize;
        bgm.volume = BGMSoundSize;
    }

    public void PlaySE(AudioType type)
    {
        AudioClip audioClip = FindAudioClip(type);
        audioSource.PlayOneShot(audioClip);
    }

    public void ChangeSize(string sound, float size)
    {
        if (sound == "SE")
            SESoundSize = size;
        else if (sound == "BGM")
            BGMSoundSize = size;
    }

    public float GetSize(string sound)
    {
        if (sound == "SE")
            return audioSource.volume;
        else if (sound == "BGM")
            return bgm.volume;

        return 0;
    }

    public void ChangeVolume(string sound)
    {
        if(sound == "SE")
            audioSource.volume = SESoundSize;
        else if(sound == "BGM")
            bgm.volume = BGMSoundSize;
    }

    private AudioClip FindAudioClip(AudioType type)
    {
        var data = audioDataList.Find(x => x.Type == type);
        if (data is null)
        {
            throw new Exception($"Audio data is null. (type: {type})");
        }

        if (data.Clip is null)
        {
            throw new Exception($"Audio clip is null. (type: {type})");
        }

        return data.Clip;
    }
}
