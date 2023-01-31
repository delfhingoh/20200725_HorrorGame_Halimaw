using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/*
 * ThisSceneManagement: This script contains FUNCTIONS that are related to SCENES. 
 * 
 * There is a KNOWN ISSUE with DONTDESTROYONLOAD. The "fix" I am using
 * is ADDITIVE SCENE which means ADDING SCENE ONTO 'DoNotUnload' SCENE.
 */
public class ThisSceneManagement : MonoBehaviour
{
    public static ThisSceneManagement thisSceneManagement;
    public string additiveSceneName;
    public string previousAdditiveSceneName;
    private bool hasMainMenuLoaded;
    private bool hasSceneUnloaded;

    private void Awake()
    {
        hasMainMenuLoaded = false;

        if (!hasMainMenuLoaded)
        {
            thisSceneManagement = this;
            LoadAdditiveScene("MainMenu");
            previousAdditiveSceneName = "MainMenu";
            hasMainMenuLoaded = true;
        }
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name != GetCurrentAdditiveSceneName())
        {
            if (SceneManager.GetSceneByName(GetCurrentAdditiveSceneName()).isLoaded)
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(GetCurrentAdditiveSceneName()));
        }
    }

    private IEnumerator Unload(string _thisSceneName)
    {
        yield return null;
        SceneManager.UnloadSceneAsync(_thisSceneName);
    }

    public void UnloadScene(string _thisSceneName)
    {
        hasSceneUnloaded = false;

        if(!hasSceneUnloaded)
        {
            StartCoroutine(Unload(_thisSceneName));

            hasSceneUnloaded = true;
        }
    }

    public void LoadAdditiveScene(int _index)
    {
        DoNotUnload.doNotUnload.DoNotUnloadStart();

        previousAdditiveSceneName = additiveSceneName;
        additiveSceneName = SceneManager.GetSceneByBuildIndex(_index).name;
        SceneManager.LoadSceneAsync(_index, LoadSceneMode.Additive);
    }

    public void LoadAdditiveScene(string _thisSceneName)
    {
        DoNotUnload.doNotUnload.DoNotUnloadStart();

        previousAdditiveSceneName = additiveSceneName;
        additiveSceneName = _thisSceneName;
        SceneManager.LoadSceneAsync(_thisSceneName, LoadSceneMode.Additive);
    }

    public AsyncOperation LoadAdditiveSceneAsync(string _thisSceneName)
    {
        DoNotUnload.doNotUnload.DoNotUnloadStart();

        previousAdditiveSceneName = additiveSceneName;
        additiveSceneName = _thisSceneName;
        return SceneManager.LoadSceneAsync(_thisSceneName, LoadSceneMode.Additive);
    }

    public int GetCurrentAdditiveSceneIndex()
    {
        return SceneManager.GetSceneByName(GetCurrentAdditiveSceneName()).buildIndex;
    }

    public string GetCurrentAdditiveSceneName()
    {
        return additiveSceneName;
    }

    public string GetPreviousAdditiveSceneName()
    {
        return previousAdditiveSceneName;
    }
}
