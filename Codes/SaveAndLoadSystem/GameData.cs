using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * GameData: This is the SCRIPT of CUSTOM CLASSES for SAVE & LOAD.
 */
[System.Serializable]
public class PlayerData
{
    public float[] position;
    public float stressLevel;
    public float vignetteIntensity;
    public bool isVignetteShown;

    public PlayerData(UnityStandardAssets.Characters.FirstPerson.FirstPersonController FPController)
    {
        position = new float[3];

        position[0] = FPController.transform.position.x;
        position[1] = FPController.transform.position.y;
        position[2] = FPController.transform.position.z;

        stressLevel = FPController.GetComponent<FirstPersonManager>().GetStressLevel();
        vignetteIntensity = FPController.GetComponent<FirstPersonManager>().GetVignetteOverlay().intensity.value;
        isVignetteShown = FPController.GetComponent<FirstPersonManager>().GetIsVignetteShown();
    }
}

[System.Serializable]
public class EnemyData
{
    public string name;
    public float[] position;
    public float[] rotation;
    public int stateID;
    public int patrolIndex;

    public EnemyData(GameObject thisEnemy)
    {
        name = thisEnemy.name;

        position = new float[3];
        rotation = new float[3];

        position[0] = thisEnemy.transform.position.x;
        position[1] = thisEnemy.transform.position.y;
        position[2] = thisEnemy.transform.position.z;

        rotation[0] = thisEnemy.transform.eulerAngles.x;
        rotation[1] = thisEnemy.transform.eulerAngles.y;
        rotation[2] = thisEnemy.transform.eulerAngles.z;

        stateID = thisEnemy.GetComponent<EnemyManager>().GetThisState();
        patrolIndex = thisEnemy.GetComponent<EnemyManager>().GetCurrentPatrolIndex();
    }
}

[System.Serializable]
public class PickableObjectData
{
    public float[] position;
    public float[] rotation;
    public string parentName;
    public string thisOBJName;

    public PickableObjectData(GameObject _thisObject)
    {
        if (_thisObject.transform.parent)
            parentName = _thisObject.transform.parent.name;
        else
            parentName = null;

        thisOBJName = _thisObject.name;

        position = new float[3];
        rotation = new float[3];

        position[0] = _thisObject.transform.position.x;
        position[1] = _thisObject.transform.position.y;
        position[2] = _thisObject.transform.position.z;

        rotation[0] = _thisObject.transform.eulerAngles.x;
        rotation[1] = _thisObject.transform.eulerAngles.y;
        rotation[2] = _thisObject.transform.eulerAngles.z;
    }
}

[System.Serializable]
public class LightObjectData
{
    public string OBJName;
    public float intensity;

    public LightObjectData(GameObject _thisLight)
    {
        OBJName = _thisLight.name;
        intensity = _thisLight.GetComponentInChildren<Light>().intensity;
    }
}

[System.Serializable]
public class LockedObjectData
{
    public string OBJName;
    public string tagName;
    public string materialName;

    public LockedObjectData(GameObject _thisLockedOBJ)
    {
        OBJName = _thisLockedOBJ.name;
        tagName = _thisLockedOBJ.tag;

        if (_thisLockedOBJ.GetComponent<ObjectOutlineScript>())
            materialName = _thisLockedOBJ.GetComponent<Renderer>().materials[1].name;
        else
            materialName = _thisLockedOBJ.GetComponent<Renderer>().material.name;
    }
}

[System.Serializable]
public class SceneData
{
    public string currentSceneName;
    public int currentSceneIndex;

    public SceneData(string _sceneName)
    {
        currentSceneName = _sceneName;
    }
}

[System.Serializable]
public class TimeData
{
    public string thisName;
    public float currentTime;

    public TimeData(GameObject _time)
    {
        thisName = _time.name;
        currentTime = _time.GetComponent<timer>().secsToFinish;
    }
}