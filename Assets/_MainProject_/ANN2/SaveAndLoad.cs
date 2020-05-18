using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveAndLoad : MonoBehaviour
{
    private const string VIDEOGAMEDATA = "videogameData";

    public static void Save<T>(T objectToSave, string key)
    {
        string path = Application.persistentDataPath + "/save/";
        Directory.CreateDirectory(path);
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(path + key, FileMode.Create))
        {
            formatter.Serialize(fileStream, objectToSave);
        }
    }

    public static void SaveVolume (float volume)
    {
        string path = Application.persistentDataPath + "/save/";
        Directory.CreateDirectory(path);
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(path + VIDEOGAMEDATA, FileMode.Create))
        {
            formatter.Serialize(fileStream, volume);
        }
    }

    public static float LoadVolume()
    {
        string path = Application.persistentDataPath + "/save/";
        BinaryFormatter formatter = new BinaryFormatter();
        float volume;
        using (FileStream fileStream = new FileStream(path + VIDEOGAMEDATA, FileMode.Open))
        {
            volume = (float)formatter.Deserialize(fileStream);
        }

        return volume;
    }

    public static bool SaveVolumeExists()
    {
        string path = Application.persistentDataPath + "/save/" + VIDEOGAMEDATA;
        return File.Exists(path);
    }


    public static T Load<T>(string key)
    {
        string path = Application.persistentDataPath + "/save/";
        BinaryFormatter formatter = new BinaryFormatter();
        T returnValue = default(T);
        using (FileStream fileStream = new FileStream(path + key, FileMode.Open))
        {
            returnValue = (T)formatter.Deserialize(fileStream);
        }

        return returnValue;
    }


    public static bool SaveExists(string key)
    {
        string path = Application.persistentDataPath + "/save/" + key;
        return File.Exists(path);
    }

    public static void DeleteAllSaveFiles()
    {
        string path = Application.persistentDataPath + "/save/";
        DirectoryInfo directory = new DirectoryInfo(path);
        directory.Delete();
    }

}
