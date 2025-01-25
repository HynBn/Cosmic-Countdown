using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{

    [Header("Slider")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;


    private void Awake()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        }
        else
        {
            musicSlider.value = 0.30f;
            sfxSlider.value = 0.30f;
        }

    }
    public void SetMusicVolume()
    {
        SoundManager.instance.SetMusicVolume(musicSlider.value);
    }

    public void SetSFXVolume()
    {
        SoundManager.instance.SetSFXVolume(sfxSlider.value);
    }


}