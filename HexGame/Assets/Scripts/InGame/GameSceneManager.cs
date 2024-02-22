using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager
{
    public static GameSceneManager instance = null;

    public GameController GameController;
    public UIController UIController;
    public int hashcode;

    public GameSceneManager()
    {
        hashcode = fff();
    }
    public static GameSceneManager GetInstance()
    {
        if (instance == null)
        {   
            instance = new GameSceneManager();
        }  
        return instance;
    }

    private int fff()
    {
        return this.GetHashCode();
    }  
}
