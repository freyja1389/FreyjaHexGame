using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New SceneManager", menuName = "Scriptable constants/Scene Manager", order = 1)]
public class SceneManager : ScriptableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

   public void LoadMainScene()
   {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
   }
}
