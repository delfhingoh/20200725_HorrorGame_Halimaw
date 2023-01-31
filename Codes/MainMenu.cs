using UnityEngine;
using System.IO;
using UnityEngine.UI;

/*
 * MainMenu: This script handles the behaviours and interactions on Main Menu.
 * The Main Menu SCENE that will be ADDED onto 'DoNotUnload' SCENE.
 */
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject loadButton;
  
    private bool isSaveFileThere;
    public GameObject volUI;

    private void Start()
    {
        isSaveFileThere = false;

        // Check if there's a save file
        if (File.Exists(SaveManager.saveManager.GetSceneDataFilePath()))
        {
            loadButton.SetActive(true);
            isSaveFileThere = true;
        }
    }


    public void PlayButton()
    {
        // Show a UI screen with a warning that this will
        // overwrite current save file.

        Debug.Log("PLAY NEW GAME. CLEAR OLD SAVE.");

        if (isSaveFileThere)
            SaveManager.saveManager.ClearSaveFile();

        ThisSceneManagement.thisSceneManagement.LoadAdditiveScene("Stage1");
        ThisSceneManagement.thisSceneManagement.UnloadScene("MainMenu");
    }

    public void LoadButton()
    {
        SaveManager.saveManager.LoadGame();
    }

    public void OptionsButton()
    {
        // Show a UI screen with audio settings
        Debug.Log("OPTIONS. GIMME THE AUDIO SETTINGS :')");
        volUI.SetActive(true);
    }

    public void QuitButton()
    {
        Debug.Log("DON'T QUIT! QUITTING...");
    
    }
    public void DoneButton()
    {
        volUI.SetActive(false);
    }
   
}
