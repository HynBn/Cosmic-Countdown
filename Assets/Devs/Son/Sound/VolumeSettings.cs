using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Header("Audiomixer")]
    [SerializeField] private AudioMixer m_Mixer;

    [Header("Slider")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;


    void Awake()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            Debug.Log("volume set: " + PlayerPrefs.GetFloat("musicVolume"));
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }


    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        m_Mixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
        //recalculated from linear to logarithmic 
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        m_Mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetMusicVolume();
        SetSFXVolume();
    }
}