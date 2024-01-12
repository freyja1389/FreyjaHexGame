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

    public event Action StartClicked;

    public void ChangeNextLevelMenuButtonText(string text)
    {
        nextLevelMenuButtonText.text = text;
    }
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
   
