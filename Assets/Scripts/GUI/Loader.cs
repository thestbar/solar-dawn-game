using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public static class Loader 
{
    // Empty LoadingMonoBehaviour class which is the component
    // that is added to the loading game object.
    private class LoadingMonoBehaviour : MonoBehaviour { }

    // Enumerator that contains every scene of the game.
    // To add a new scene and be able to redirect the engine
    // to this scene, it needs to be added on this enumerator.
    public enum Scene
    {
        Level1,
        Loading,
        MainMenu,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
        Level7,
        Level8,
        Level9,
        MissionsMenu
    }

    private static Action onLoaderCallback;
    private static AsyncOperation loadingAsyncOperation;

    public static void Load(Scene scene)
    {
        // Set the loader callback action to load the target scene.
        onLoaderCallback = () =>
        {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
        };
        // Load the loading scene.
        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    // Co-routine that loads the selected screen.
    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        yield return null;
        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        yield return new WaitForSeconds(10);
        while (!loadingAsyncOperation.isDone)
        {
            yield return null;
        }
    }

    // Function that returns the loading progress of the loading process.
    public static float GetLoadingProgress()
    {
        if (loadingAsyncOperation != null)
        {
            return loadingAsyncOperation.progress;
        }
        else
        {
            return 1.0f;
        }
    }

    public static void LoaderCallback()
    {
        // Triggered after the first Update which lets the screen refresh.
        // Execute the loader callback action which will load the target scene.
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
