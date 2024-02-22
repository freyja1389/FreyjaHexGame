using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneReloader : MonoBehaviour
{
    public event Action SceneWasLoaded;
    public event Action SceneWasUnloaded;

    public void LoadAdditiveSceneWithCorutine(string sceneName)
    {
        StartCoroutine(LoadNewAdditiveScene(sceneName));
    }

    public void LoadSceneWithCorutine(string sceneName)
    {
        StartCoroutine(LoadNewScene(sceneName));
    }

    public void UnLoadSceneWithCorutine(string sceneName)
    {
        StartCoroutine(UnloadScene(sceneName));
    }

    private IEnumerator LoadNewAdditiveScene(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (asyncLoad.isDone)
        {
            Debug.Log("scene was loaded asyncly!");
            SceneWasLoaded?.Invoke();
        }
    }

    private IEnumerator LoadNewScene(string sceneName)
    {
        Debug.Log("AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);");
        var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            Debug.Log("yield return null;");
            yield return null;
        }


        //if (asyncLoad.isDone)
        //{
            Debug.Log("scene was loaded asyncly!");
            SceneWasLoaded?.Invoke();
        //}
    }

    private IEnumerator UnloadScene(string sceneName)
    {
        var asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        if (asyncUnload.isDone)
        {
            Debug.Log("scene was unloaded asyncly!");
            SceneWasUnloaded?.Invoke();
        }
    }
}


