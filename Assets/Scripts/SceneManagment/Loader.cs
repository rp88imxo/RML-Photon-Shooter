using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{

    public class LoadingMonoBehaviour : MonoBehaviour
    {

    }

    public enum Scene
    {
        GameScene1,
        MenuScene,
        LoadingScene
    }

    private static Action onLoaderCallback; 
    private static bool isFirstLaunch = true;
    private static AsyncOperation asyncOperation;

    public static float LoadingProgress 
    {
        get
        {
            if (asyncOperation != null)
            {
                return asyncOperation.progress;
            }
            else
            {
                return 1f;
            }
        } 
    }

    public static void Load(Scene scene)
    {
        onLoaderCallback = () =>
        {
            GameObject loadingGameObject = new GameObject("loading Game Object");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
        };
        
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
        isFirstLaunch = false;
    }

    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        asyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        //yield return null;
       
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    public static float GetLoadingProgress()
    {
        if (asyncOperation != null)
        {
            return asyncOperation.progress;
        }
        else
        {
            return 1f;
        }

       
    }

    public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
