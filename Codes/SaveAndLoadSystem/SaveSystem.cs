using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/*
 * SaveSystem: This might NOT BE THE MOST OPTIMISED saving SOLUTION as there
 * might be better ones out there but it works well for our need.
 * 
 * This script HANDLES the ACTUAL SAVING and LOADING of the game.
 * 
 * CURRENT things that requires SAVE & LOAD ARE:
 * PLAYER
 * PICKABLEOBJECT
 * LIGHTOBJECT
 * SCENE
 * ENEMY
 * TIMER
 */
public static class SaveSystem 
{
    // Overwrite
    public static void ClearSaveFile(string[] _path)
    {
        for(int i = 0; i < _path.Length; i++)
        {
            Debug.Log("CLEARING + " + _path[i]);

            if (File.Exists(_path[i]))
                File.Delete(_path[i]);
        }
    }

    // Scene Data
    public static void SaveScene(string _sceneName, string _path)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = _path;
        FileStream stream = new FileStream(path, FileMode.Create);

        SceneData sceneData = new SceneData(_sceneName);

        formatter.Serialize(stream, sceneData);
        stream.Close();
    }

    public static SceneData LoadScene(string _path)
    {
        string path = _path;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SceneData sceneData = formatter.Deserialize(stream) as SceneData;
            stream.Close();

            return sceneData;
        }
        else
        {
            Debug.LogError("Scene save file not found in " + path);
            return null;
        }
    }

    // Timer Data
    public static void SaveTime(GameObject _time, string _path)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = _path;
        FileStream stream = new FileStream(path, FileMode.Create);

        TimeData timeData = new TimeData(_time);

        formatter.Serialize(stream, timeData);
        stream.Close();
    }

    public static TimeData LoadTime(string _path)
    {
        string path = _path;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            TimeData timeData = formatter.Deserialize(stream) as TimeData;
            stream.Close();

            return timeData;
        }
        else
        {
            Debug.LogError("Time save file not found in " + path);
            return null;
        }
    }

    // Player Data
    public static void SavePlayer(UnityStandardAssets.Characters.FirstPerson.FirstPersonController FPController, string _path)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = _path;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData playerData = new PlayerData(FPController);

        formatter.Serialize(stream, playerData);
        stream.Close();
    }

    public static PlayerData LoadPlayer(string _path)
    {
        string path = _path;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData playerData = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return playerData;
        }
        else
        {
            Debug.LogError("Player save file not found in " + path);
            return null;
        }
    }

    // Enemy Data
    public static void SaveEnemy(GameObject[] thisEnemy, string _path)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = _path;
        FileStream stream = new FileStream(path, FileMode.Create);

        EnemyData[] enemyData = new EnemyData[thisEnemy.Length];

        for (int i = 0; i < thisEnemy.Length; i++)
        {
            EnemyData tempObjectData = new EnemyData(thisEnemy[i]);
            enemyData[i] = tempObjectData;
        }

        formatter.Serialize(stream, enemyData);
        stream.Close();
    }

    public static EnemyData[] LoadEnemy(string _path)
    {
        string path = _path;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            EnemyData[] enemyData = formatter.Deserialize(stream) as EnemyData[];
            stream.Close();

            return enemyData;
        }
        else
        {
            Debug.LogError("Enemy save file not found in " + path);
            return null;
        }
    }

    // Object Data
    // Locked Objects
    public static void SaveLockedObjectData(GameObject[] _thisObjectArray, string _path)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = _path;
        FileStream stream = new FileStream(path, FileMode.Create);

        LockedObjectData[] lockedObjectDataArray = new LockedObjectData[_thisObjectArray.Length];

        for (int i = 0; i < _thisObjectArray.Length; i++)
        {
            LockedObjectData tempObjectData = new LockedObjectData(_thisObjectArray[i]);
            lockedObjectDataArray[i] = tempObjectData;
        }

        formatter.Serialize(stream, lockedObjectDataArray);
        stream.Close();
    }

    public static LockedObjectData[] LoadLockedObjectData(string _path)
    {
        string path = _path;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LockedObjectData[] lockedObjectDataArray = formatter.Deserialize(stream) as LockedObjectData[];
            stream.Close();

            return lockedObjectDataArray;
        }
        else
        {
            Debug.LogError("Locked object save file not found in " + path);
            return null;
        }

    }

    // Light Objects
    public static void SaveLightObjectData(GameObject[] _thisObjectArray, string _path)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = _path;
        FileStream stream = new FileStream(path, FileMode.Create);

        LightObjectData[] lightObjectDataArray = new LightObjectData[_thisObjectArray.Length];

        for (int i = 0; i < _thisObjectArray.Length; i++)
        {
            LightObjectData tempObjectData = new LightObjectData(_thisObjectArray[i]);
            lightObjectDataArray[i] = tempObjectData;
        }

        formatter.Serialize(stream, lightObjectDataArray);
        stream.Close();
    }

    public static LightObjectData[] LoadLightObjectData(string _path)
    {
        string path = _path;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LightObjectData[] lightObjectData = formatter.Deserialize(stream) as LightObjectData[];
            stream.Close();

            return lightObjectData;
        }
        else
        {
            Debug.LogError("Light object save file not found in " + path);
            return null;
        }

    }

    // Pickable Objects
    public static void SavePickableObjectData(GameObject[] _thisObjectArray, string _path)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = _path;
        FileStream stream = new FileStream(path, FileMode.Create);

        PickableObjectData[] pickableObjectsDataArray = new PickableObjectData[_thisObjectArray.Length];

        for(int i = 0; i < _thisObjectArray.Length; i++)
        {
            PickableObjectData tempObjectData = new PickableObjectData(_thisObjectArray[i]);
            pickableObjectsDataArray[i] = tempObjectData;
        }

        formatter.Serialize(stream, pickableObjectsDataArray);
        stream.Close();
    }

    public static PickableObjectData[] LoadPickableObjectData(string _path)
    {
        string path = _path;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PickableObjectData[] pickableObjectData = formatter.Deserialize(stream) as PickableObjectData[];
            stream.Close();

            return pickableObjectData;
        }
        else
        {
            Debug.LogError("Pickable object save file not found in " + path);
            return null;
        }

    }
}
