using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public abstract class BaseMenuControls : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject SettingsPanel;
    public bool isFullScreen;
    public AudioMixer Audio;
    public AudioSource sorceAud;
    public AudioClip muz1;
    public AudioClip muz2;
    [SerializeField]
    protected SceneManager sceneManager;
    //public GameObject MenuPanel;

    public void PressedExit()
    {
        Application.Quit();
        Debug.Log("Exit pressed!");
    }

    public virtual void PressedStart()
    {
        sorceAud.clip = muz2;
        sorceAud.Play();
        sceneManager.LoadMainScene();
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

    public void ActivateMenuPanel(bool param)
    {
        MenuPanel.SetActive(param);
    }
}
