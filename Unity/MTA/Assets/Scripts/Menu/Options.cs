using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] TMP_Dropdown graphicsDropdown;
    [SerializeField] Toggle fullscreenToggle;
    public AudioMixer music;
    public AudioMixer sfx;

    void Start()
    {
        Debug.Log(PlayerPrefs.GetString("fullscreenMode"));
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);     
            Load();
        }
        if(!PlayerPrefs.HasKey("sfxVolume"))
        {
            PlayerPrefs.SetFloat("sfxVolume", 1);
            Load();
        }
        if(!PlayerPrefs.HasKey("qualityLevel"))
        {
            PlayerPrefs.SetInt("qualityLevel", 2);
            Load();
        }
        if(!PlayerPrefs.HasKey("fullscreenMode"))
        {
            PlayerPrefs.SetString("fullscreenMode", "true");
            Load();
        }
        
        Load();
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

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log(QualitySettings.GetQualityLevel());
        Save(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Save(isFullscreen);
    }

    private void Load()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        graphicsDropdown.value = PlayerPrefs.GetInt("qualityLevel");
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("qualityLevel"));

        Screen.fullScreen.Equals(PlayerPrefs.GetString("fullscreenMode"));
        if(PlayerPrefs.GetString("fullscreenMode") == "True")
        {
            fullscreenToggle.isOn = true;
        }
        else
        {
            fullscreenToggle.isOn = false;
        }
    }
    
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
    }

    private void Save(int quality)
    {
        PlayerPrefs.SetInt("qualityLevel", quality);
    }

    private void Save(bool isFullscreen)
    {
        PlayerPrefs.SetString("fullscreenMode", isFullscreen.ToString());
    }
}
