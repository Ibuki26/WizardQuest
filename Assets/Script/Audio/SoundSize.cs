using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSize : MonoBehaviour
{
    [SerializeField] private string sound;
    private Slider slider;
    

    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = AudioManager.Instance.GetSize(sound);
    }
    
    public void ChangeSoundSize()
    {
        AudioManager.Instance.ChangeSize(sound, slider.value);
        AudioManager.Instance.ChangeVolume(sound);
    }
}
