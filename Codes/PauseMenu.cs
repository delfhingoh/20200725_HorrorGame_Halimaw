using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PauseMenu: This script handles the behaviours and interactions of
 * PAUSE MENU. As ALL GAMEPLAY SCENES requires this OBJECT, it has been placed
 * in 'DoNotUnload' SCENE.
 */
public class PauseMenu : MonoBehaviour
{
    [Header("")]
    [Header("PAUSE")]
    public GameObject pauseMenu;

    private void Start()
    {
        if (!pauseMenu)
            pauseMenu = GameObject.Find("Pause_Canvas");
    }

    public void ShowUIScreen()
    {
        pauseMenu.SetActive(true);

        DoNotUnload.doNotUnload.UnlockMouseCursor();
        DoNotUnload.doNotUnload.LockPlayerMovement();
    }

    public void CloseUIScreen()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;

        DoNotUnload.doNotUnload.LockMouseCursor();
        DoNotUnload.doNotUnload.UnlockPlayerMovement();
    }

    public void SaveButton()
    {
        SaveManager.saveManager.SaveGame();

        Debug.Log("SAVING IN PROCESS");
    }

    public void QuitButton()
    {
        Debug.Log("QUIT. PROMPT THAT THE PROGRESS WILL NOT BE SAVED");
        CloseUIScreen();

        DoNotUnload.doNotUnload.FPController.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.m_cursorIsLocked = false;
        DoNotUnload.doNotUnload.FPController.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_MouseLook.SetCursorLock(false);

        string tempName = ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName();

        ThisSceneManagement.thisSceneManagement.LoadAdditiveScene("MainMenu");
        ThisSceneManagement.thisSceneManagement.UnloadScene(tempName);
    }
}
