using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;

public abstract class BaseMenuControls : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject SettingsPanel;
    public bool isFullScreen;
    public AudioMixer Audio;
    public AudioSource sorceAud;
    public AudioClip muz1;
    public AudioClip muz2;
    public SceneReloader SceneReloader;

    public event Action NextLevelClicked;
    //public event Action StartClicked;


    public void PressedExit()
    {
        Application.Quit();
        Debug.Log("Exit pressed!");
    }

    public virtual void PressedStart()
    {
        sorceAud.clip = muz2;
        sorceAud.Play();
        
        SceneReloader.SceneWasLoaded += WhenUISceneWasLoaded;
        SceneReloader.LoadAdditiveSceneWithCorutine("UIScene");
    }

   
    public virtual void PressedNextLevel()
    {
        sorceAud.clip = muz2;
        sorceAud.Play();
        SceneReloader.SceneWasUnloaded += WhenSceneWasUnloaded;
        SceneReloader.UnLoadSceneWithCorutine("SampleScene");
        NextLevelClicked.Invoke();       
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

    private void WhenSceneWasUnloaded()
    {
        SceneReloader.SceneWasUnloaded -= WhenSceneWasUnloaded;
        SceneReloader.SceneWasLoaded += WhenSceneWasLoaded;
        SceneReloader.LoadAdditiveSceneWithCorutine("SampleScene");
    }

    private void WhenSceneWasLoaded()
    {
        SceneReloader.SceneWasUnloaded -= WhenSceneWasLoaded;
        if (SceneManager.GetSceneByName("MenuScene").isLoaded)
        {
            SceneReloader.UnLoadSceneWithCorutine("MenuScene");
        }
    }

   /* private void WhenMenuSceneWasUnloaded()
    {
        SceneReloader.SceneWasLoaded -= WhenMenuSceneWasUnloaded;
        NextLevelClicked?.Invoke();
    }*/

    private void WhenUISceneWasLoaded()
    {
        SceneReloader.SceneWasLoaded -= WhenUISceneWasLoaded;
        SceneReloader.SceneWasLoaded += WhenSceneWasLoaded;
        SceneReloader.LoadAdditiveSceneWithCorutine("SampleScene");
    }
}
