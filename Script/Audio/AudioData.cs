using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(AudioData), menuName = "ScriptableObjects/AudioData")]
public class AudioData : ScriptableObject
{
    [SerializeField] private AudioType type;
    [SerializeField] private AudioClip audioClip;

    public AudioType Type => type;
    public AudioClip Clip => audioClip;
}
