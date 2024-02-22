using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InGameMenuControls : BaseMenuControls
{
    [SerializeField]
    private TMPro.TextMeshProUGUI nextLevelMenuButtonText;

    public GameObject NextLevelMenuPanel;
    public GameObject BackMenuPanel;



    public void Start()
    {
       
    }

    public void ChangeNextLevelMenuButtonText(string text)
    {
        nextLevelMenuButtonText.text = text;
    }

    public void BackToGame()
    {
        BackMenuPanel.SetActive(true);
        ActivateMenuPanel(false);
    }

    public override void PressedNextLevel()
    {
        NextLevelMenuPanel.SetActive(false);
        base.PressedNextLevel();
       // base.SceneReloaded += WhenSceneReload;
        //

    }

    public override void PressedStart()
    {
        base.PressedStart();
       // StartClicked?.Invoke();
    }

    public void ActivateBackMenuPanel(bool param)
    {
        BackMenuPanel.SetActive(param);
    }

    public void BackToMenu()
    {
        BackMenuPanel.SetActive(false);        
        ActivateMenuPanel(true);
    }
}
   
