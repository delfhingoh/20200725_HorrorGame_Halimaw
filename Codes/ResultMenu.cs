using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ResultMenu : MonoBehaviour
{
    [SerializeField] private GameObject winUIScreen;
    [SerializeField] private GameObject loseUIScreen;

    private FirstPersonManager FPManager;
    private timer thisTime;
    private bool isReloadingScene;

    private void Start()
    {
        isReloadingScene = false;
        thisTime = GetComponent<timer>();
    }

    private void Update()
    {
        if(ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() != "MainMenu")
        {
            if (!FPManager)
                FPManager = DoNotUnload.doNotUnload.FPController.GetComponent<FirstPersonManager>();

            // LOSE UI
            if ((FPManager.GetStressLevel() >= FPManager.GetMaxStressLevel() || thisTime.GetSecsToFinishTime() <= 0f) && !isReloadingScene)
            {
                Time.timeScale = 0f;
                ShowLoseUIScreen();
            }
        }
    }

    public void ShowWinUIScreen()
    {
        winUIScreen.SetActive(true);

        DoNotUnload.doNotUnload.LockPlayerMovement();
        DoNotUnload.doNotUnload.UnlockMouseCursor();
    }

    public void ShowLoseUIScreen()
    {
        loseUIScreen.SetActive(true);

        DoNotUnload.doNotUnload.LockPlayerMovement();
        DoNotUnload.doNotUnload.UnlockMouseCursor();
    }

    public void MainMenuButton()
    {
        Time.timeScale = 1f;
        DoNotUnload.doNotUnload.LockPlayerMovement();
        DoNotUnload.doNotUnload.UnlockMouseCursor();

        if (ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() == "Stage1")
            ThisSceneManagement.thisSceneManagement.UnloadScene("Stage1");
        else if (ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() == "Stage2")
            ThisSceneManagement.thisSceneManagement.UnloadScene("Stage2");
        else if (ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() == "Stage3")
            ThisSceneManagement.thisSceneManagement.UnloadScene("Stage3");

        ThisSceneManagement.thisSceneManagement.LoadAdditiveScene("MainMenu");

        winUIScreen.SetActive(false);
        loseUIScreen.SetActive(false);

        isReloadingScene = true;
        StartCoroutine(WaitForThis());
    }

    public void RetryButton()
    {
        Time.timeScale = 1f;
        DoNotUnload.doNotUnload.LockPlayerMovement();
        DoNotUnload.doNotUnload.UnlockMouseCursor();

        winUIScreen.SetActive(false);
        loseUIScreen.SetActive(false);

        thisTime.secsToFinish = thisTime.GetTimeLeftForPrevStage();

        // Clear SAVED FILE
        if (File.Exists(SaveManager.saveManager.GetSceneDataFilePath()))
            SaveManager.saveManager.ClearSaveFile();

        if (ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() == "Stage1")
        {
            ThisSceneManagement.thisSceneManagement.UnloadScene("Stage1");
            ThisSceneManagement.thisSceneManagement.LoadAdditiveScene("Stage1");
            isReloadingScene = true;

            StartCoroutine(WaitForThis());
        }
        else if (ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() == "Stage2")
        {
            ThisSceneManagement.thisSceneManagement.UnloadScene("Stage2");
            ThisSceneManagement.thisSceneManagement.LoadAdditiveScene("Stage2");
            isReloadingScene = true;

            StartCoroutine(WaitForThis());
        }
        else if (ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() == "Stage3")
        {
            ThisSceneManagement.thisSceneManagement.UnloadScene("Stage3");
            ThisSceneManagement.thisSceneManagement.LoadAdditiveScene("Stage3");
            isReloadingScene = true;

            StartCoroutine(WaitForThis());
        }
    }

    private IEnumerator WaitForThis()
    {
        yield return new WaitForSeconds(1f);
        isReloadingScene = false;
    }
}
