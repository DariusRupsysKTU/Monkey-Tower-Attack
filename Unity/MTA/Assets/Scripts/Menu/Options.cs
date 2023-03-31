using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    public AudioMixer music;
    public AudioMixer sfx;

    void Start()
    {
        if(!PlayerPrefs.HasKey("musicVolume") || !PlayerPrefs.HasKey("sfxVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            PlayerPrefs.SetFloat("sfxVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void SetMusicVolume(float volume)
    {
        music.SetFloat("volume", Mathf.Log10(volume)*20);
        Save();
    }

    public void SetSFXVolume(float volume)
    {
        sfx.SetFloat("volume", Mathf.Log10(volume) * 20);
        Save();
    }

    private void Load()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
    }
    
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
    }
}
