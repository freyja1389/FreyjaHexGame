using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Audio;

public class MenuControls : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject NextLevelMenuPanel;
    public GameObject SettingsPanel;
    public bool isFullScreen;
    public AudioMixer Audio;
    public AudioSource sorceAud;
    public AudioClip muz1;
    public AudioClip muz2;

    public event Action StartClicked;
    public void PressedExit()
    {
        Application.Quit();
        Debug.Log("Exit pressed!");
    }

    public void PressedStart()
    {
        sorceAud.clip = muz2;
        sorceAud.Play();
        StartClicked?.Invoke();
    }

    public void PressedNextLevel()
    {
        StartClicked?.Invoke();
        NextLevelMenuPanel.SetActive(false);
    }

    public void PressedSettings()
    {
        MenuPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }   

    public void PressedBackSettings()
    {
        SettingsPanel.SetActive(false);
        MenuPanel.SetActive(true);
    }

    public void PressedBack()
    {
        MenuPanel.SetActive(false);
    }

    public void FullScreenToggle()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
    }

    public void AudioVolume(float sliderValue)
    {
        Audio.SetFloat("masterVolume", sliderValue);
        
    }
}
