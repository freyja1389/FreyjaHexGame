using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InGameMenuControls : BaseMenuControls
{
    public GameObject NextLevelMenuPanel;
    public GameObject BackMenuPanel;


    public event Action StartClicked;
    public void BackToGame()
    {
        BackMenuPanel.SetActive(true);
        ActivateMenuPanel(false);
    }
    public void PressedNextLevel()
    {
        StartClicked?.Invoke();
        NextLevelMenuPanel.SetActive(false);
    }

    public override void PressedStart()
    {
        StartClicked?.Invoke();
        base.PressedStart();
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
   
