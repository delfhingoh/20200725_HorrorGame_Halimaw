using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * SaveManager: This script is to contains PUBLIC FUNCTIONS to SAVE OR LOAD game. This is so that
 * other SCRIPTS handling UI behaviours (ButtonPressed) can access the functions.
 */
public class SaveManager : MonoBehaviour
{
    public static SaveManager saveManager;
    public bool isSaveFileLoaded;

    private string sceneDataPath;
    private string timeDataPath;
    private string playerDataPath;
    private string pickableObjectDataPath;
    private string lightObjectDataPath;
    private string enemyDataPath;
    private string lockedObjectDataPath;

    private GameObject[] enemy;
    private GameObject[] pickableObjectArray;
    private GameObject[] lightObjectArray;
    private GameObject[] lockedObjectArray;
    private GameObject tutorialCanvas;
    private GameObject timeOBJ;
    private bool isSceneLoaded;

    private void Awake()
    {
        saveManager = this;
    }

    private void Start()
    {
        isSceneLoaded = false;
        isSaveFileLoaded = false;

        sceneDataPath = Application.persistentDataPath + "/sceneData.halimaw";
        timeDataPath = Application.persistentDataPath + "/timeData.halimaw";
        playerDataPath = Application.persistentDataPath + "/playerData.halimaw";
        pickableObjectDataPath = Application.persistentDataPath + "/pickableObjectData.halimaw";
        lightObjectDataPath = Application.persistentDataPath + "/lightObjectData.halimaw";
        lockedObjectDataPath = Application.persistentDataPath + "/lockedObjectData.hallimaw";
        enemyDataPath = Application.persistentDataPath + "/enemyData.halimaw";
    }

    private void Update()
    {
        if (isSceneLoaded)
        {
            Debug.Log("LOADING SAVED FILES.");

            DoNotUnload.doNotUnload.DoNotUnloadStart();

            if (ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() != "Stage1")
                LoadSavedEnemy();

            LoadSavedPlayer();
            LoadSavedTime();
            LoadSavedPickableObject();
            LoadSavedLockedObject();
            LoadSavedLightObject();

            isSceneLoaded = false;
            isSaveFileLoaded = true;
        }
    }

    public void SaveGame()
    {
        SaveSystem.SaveScene(ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName(), sceneDataPath);
        SaveSystem.SavePlayer(DoNotUnload.doNotUnload.FPController.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>(), playerDataPath);

        timeOBJ = GameObject.Find("GameManager");
        SaveSystem.SaveTime(timeOBJ, timeDataPath);

        pickableObjectArray = GameObject.FindGameObjectsWithTag("Pickable");
        SaveSystem.SavePickableObjectData(pickableObjectArray, pickableObjectDataPath);

        lightObjectArray = GameObject.FindGameObjectsWithTag("Light");
        SaveSystem.SaveLightObjectData(lightObjectArray, lightObjectDataPath);

        ThisIsLocked[] tempArray = FindObjectsOfType(typeof(ThisIsLocked)) as ThisIsLocked[];
        lockedObjectArray = new GameObject[tempArray.Length];

        for (int i = 0; i < tempArray.Length; i++)
        {
            lockedObjectArray[i] = tempArray[i].gameObject;
        }
        SaveSystem.SaveLockedObjectData(lockedObjectArray, lockedObjectDataPath);

        if (ThisSceneManagement.thisSceneManagement.GetCurrentAdditiveSceneName() != "Stage1")
        {
            enemy = GameObject.FindGameObjectsWithTag("Enemy");
            SaveSystem.SaveEnemy(enemy, enemyDataPath);
        }

        Debug.Log("SAVED.");
    }

    public void LoadGame()
    {
        StartCoroutine(LoadSavedScene());   
    }

    public void ClearSaveFile()
    {
        string[] pathArray = new string[7];
        pathArray[0] = sceneDataPath;
        pathArray[1] = playerDataPath;
        pathArray[2] = pickableObjectDataPath;
        pathArray[3] = enemyDataPath;
        pathArray[4] = timeDataPath;
        pathArray[5] = lightObjectDataPath;
        pathArray[6] = lockedObjectDataPath;

        isSaveFileLoaded = false;
        SaveSystem.ClearSaveFile(pathArray);   
    }

    public string GetSceneDataFilePath()
    {
        return sceneDataPath;
    }

    private IEnumerator LoadSavedScene()
    {
        SceneData sceneData = SaveSystem.LoadScene(sceneDataPath);

        string savedSceneName = "";

        savedSceneName = sceneData.currentSceneName;

        // Load the saved scene by name
        AsyncOperation asyncLoad = ThisSceneManagement.thisSceneManagement.LoadAdditiveSceneAsync(savedSceneName);
        while (!asyncLoad.isDone)
        {
            isSceneLoaded = false;
            yield return null;
        }

        ThisSceneManagement.thisSceneManagement.UnloadScene("MainMenu");
        isSceneLoaded = true;

        Debug.Log("SCENE LOADED");
    }

    private void LoadSavedTime()
    {
        TimeData timeData = SaveSystem.LoadTime(timeDataPath);

        timeOBJ = GameObject.Find(timeData.thisName);
        timeOBJ.GetComponent<timer>().secsToFinish = timeData.currentTime;
    }

    private void LoadSavedPlayer()
    {
        PlayerData playerData = SaveSystem.LoadPlayer(playerDataPath);

        Vector3 tempVectorContainer;
        tempVectorContainer.x = playerData.position[0];
        tempVectorContainer.y = playerData.position[1];
        tempVectorContainer.z = playerData.position[2];
        DoNotUnload.doNotUnload.FPController.transform.position = tempVectorContainer;

        DoNotUnload.doNotUnload.FPController.GetComponent<FirstPersonManager>().SetStressLevel(playerData.stressLevel);
        DoNotUnload.doNotUnload.FPController.GetComponent<FirstPersonManager>().SetStressSlider(playerData.stressLevel);
        DoNotUnload.doNotUnload.FPController.GetComponent<FirstPersonManager>().SetIsVignetteShown(playerData.isVignetteShown);
        DoNotUnload.doNotUnload.FPController.GetComponent<FirstPersonManager>().GetVignetteOverlay().intensity.value = playerData.vignetteIntensity;
    }

    private void LoadSavedEnemy()
    {
        EnemyData[] enemyDataTemp = SaveSystem.LoadEnemy(enemyDataPath);

        GameObject tempOBJ = null;
        Vector3 tempVectorContainer = Vector3.zero;

        for(int i = 0; i < enemyDataTemp.Length; i++)
        {
            tempOBJ = GameObject.Find(enemyDataTemp[i].name);

            // Position
            tempVectorContainer.x = enemyDataTemp[i].position[0];
            tempVectorContainer.y = enemyDataTemp[i].position[1];
            tempVectorContainer.z = enemyDataTemp[i].position[2];
            tempOBJ.transform.position = tempVectorContainer;

            // Rotation
            tempVectorContainer.x = enemyDataTemp[i].rotation[0];
            tempVectorContainer.y = enemyDataTemp[i].rotation[1];
            tempVectorContainer.z = enemyDataTemp[i].rotation[2];
            tempOBJ.transform.eulerAngles = tempVectorContainer;

            // State
            tempOBJ.GetComponent<EnemyManager>().SetThisState(enemyDataTemp[i].stateID);
            // Patrol
            tempOBJ.GetComponent<EnemyManager>().SetCurrentPatrolIndex(enemyDataTemp[i].patrolIndex);
        }
    }

    private void LoadSavedLockedObject()
    {
        LockedObjectData[] lockedObjectDataTemp = SaveSystem.LoadLockedObjectData(lockedObjectDataPath);

        GameObject tempOBJ = null;
        Material[] thisMat = null;

        for(int i = 0; i < lockedObjectDataTemp.Length; i++)
        {
            tempOBJ = GameObject.Find(lockedObjectDataTemp[i].OBJName);
            tempOBJ.tag = lockedObjectDataTemp[i].tagName;
            thisMat = tempOBJ.GetComponent<Renderer>().materials;

            if (tempOBJ.GetComponent<ObjectOutlineScript>() && tempOBJ.tag == "Unlocked")
                thisMat[1] = Resources.Load(lockedObjectDataTemp[i].materialName) as Material;

            tempOBJ.GetComponent<Renderer>().materials = thisMat;
        }
    }

    private void LoadSavedLightObject()
    {
        LightObjectData[] lightObjectDataTemp = SaveSystem.LoadLightObjectData(lightObjectDataPath);

        GameObject tempOBJ = null;

        for(int i = 0; i < lightObjectDataTemp.Length; i++)
        {
            tempOBJ = GameObject.Find(lightObjectDataTemp[i].OBJName);
            tempOBJ.GetComponentInChildren<Light>().intensity = lightObjectDataTemp[i].intensity;
        }
    }

    private void LoadSavedPickableObject()
    {
        PickableObjectData[] pickableObjectDataTemp = SaveSystem.LoadPickableObjectData(pickableObjectDataPath);

        GameObject tempOBJ = null;
        GameObject tempParentOBJ = null;
        Vector3 tempVectorContainer = Vector3.zero;

        for(int i = 0; i < pickableObjectDataTemp.Length; i++)
        {
            tempOBJ = GameObject.Find(pickableObjectDataTemp[i].thisOBJName);
            tempParentOBJ = GameObject.Find(pickableObjectDataTemp[i].parentName);

            // Position
            tempVectorContainer.x = pickableObjectDataTemp[i].position[0];
            tempVectorContainer.y = pickableObjectDataTemp[i].position[1];
            tempVectorContainer.z = pickableObjectDataTemp[i].position[2];
            tempOBJ.transform.position = tempVectorContainer;

            // Rotation
            tempVectorContainer.x = pickableObjectDataTemp[i].rotation[0];
            tempVectorContainer.y = pickableObjectDataTemp[i].rotation[1];
            tempVectorContainer.z = pickableObjectDataTemp[i].rotation[2];
            tempOBJ.transform.eulerAngles = tempVectorContainer;

            // Parent
            if (tempParentOBJ != null)
            {
                if (tempParentOBJ.name == "RightGrabArea")
                {
                    DoNotUnload.doNotUnload.FPController.GetComponent<FirstPersonInteraction>().SetIsRightHandFull(true);
                    tempOBJ.GetComponent<PickMeUp>().SetIsPickUp(true);
                    tempOBJ.GetComponent<ICanBeInteracted<GameObject>>().PickableInteraction(tempParentOBJ.gameObject);
                }
                else if (tempParentOBJ.name == "LeftGrabArea")
                {
                    DoNotUnload.doNotUnload.FPController.GetComponent<FirstPersonInteraction>().SetIsLeftHandFull(true);
                    tempOBJ.GetComponent<PickMeUp>().SetIsPickUp(true);
                    tempOBJ.GetComponent<ICanBeInteracted<GameObject>>().PickableInteraction(tempParentOBJ.gameObject);
                }

                tempOBJ.transform.parent = tempParentOBJ.transform;
            }
            else
            {
                tempOBJ.GetComponent<Collider>().enabled = true;
            }
        }
    }
}
