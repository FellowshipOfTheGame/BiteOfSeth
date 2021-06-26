using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



public static class SaveSystem
{

    // 3 Saves
    public static PlayerData[] savedGames = {null, null, null};
    private static string saveName = "/flamingo.cafe";


    public static void Save()
    {

        savedGames[PlayerData.current.id] = PlayerData.current;

        BinaryFormatter formatter = new BinaryFormatter();

        string path;
        #if UNITY_WEBGL
            path = System.IO.Path.Combine("/idbfs", Application.productName);
        #else   
            path = Application.persistentDataPath + SaveSystem.saveName;
        #endif

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, SaveSystem.savedGames);

        stream.Close();

    }

    public static void EraseSave(int id)
    {

        savedGames[id] = null;

        BinaryFormatter formatter = new BinaryFormatter();

        string path;
        #if UNITY_WEBGL
            path = System.IO.Path.Combine("/idbfs", Application.productName);
        #else
            path = Application.persistentDataPath + SaveSystem.saveName;
        #endif

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, SaveSystem.savedGames);

        stream.Close();

    }

    public static void Load()
    {

        string path;
        #if UNITY_WEBGL
            path = System.IO.Path.Combine("/idbfs", Application.productName);
        #else
            path = Application.persistentDataPath + SaveSystem.saveName;
        #endif

        if (File.Exists(path)) {
            
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            savedGames = formatter.Deserialize(stream) as PlayerData[];

            stream.Close();

        } else {
            Debug.Log("Save file not found in " + path);
        }

    }

    public static void Delete()
    {

        string path;
        #if UNITY_WEBGL
                path = System.IO.Path.Combine("/idbfs", Application.productName);
        #else
                path = Application.persistentDataPath + SaveSystem.saveName;
        #endif
        
        File.Delete(path);
       
        Debug.Log("Save successfully deleted!");
    }

}
