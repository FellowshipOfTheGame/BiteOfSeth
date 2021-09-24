using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem
{
    
    public static GameData generalData = null;
    private static string generalSaveName = "/cafe.flamingo";

    // Gameplay purpose saving : 3 Saves
    public static PlayerData[] savedGames = {null, null, null};
    private static string saveName = "/flamingo.cafe";

    public static void SaveGeneral()
    {

        generalData = GameData.generalSave;

        BinaryFormatter formatter = new BinaryFormatter();

        string pathGeneral;

        pathGeneral = Application.persistentDataPath + SaveSystem.generalSaveName;

        /*#if UNITY_WEBGL
            pathGeneral = System.IO.Path.Combine("/idbfs", Application.productName);
        #else
            pathGeneral = Application.persistentDataPath + SaveSystem.generalSaveName;
        #endif*/

        FileStream stream = new FileStream(pathGeneral, FileMode.Create);

        formatter.Serialize(stream, generalData);

        stream.Close();

    }

    public static void Save()
    {

        savedGames[PlayerData.current.id] = PlayerData.current;

        BinaryFormatter formatter = new BinaryFormatter();
        
        string path;
        path = Application.persistentDataPath + saveName;

        /*#if UNITY_WEBGL
            path = System.IO.Path.Combine("/idbfs", Application.productName);
        #else   
            path = Application.persistentDataPath + saveName;
        #endif*/

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, savedGames);

        stream.Close();

    }

    public static void Load()
    {

        string pathGeneral;
        string path;

        path = Application.persistentDataPath + saveName;
        pathGeneral = Application.persistentDataPath + generalSaveName;

        /*#if UNITY_WEBGL
            path = System.IO.Path.Combine("/idbfs", Application.productName);
            pathGeneral = System.IO.Path.Combine("/idbfs", Application.productName);
        #else
            path = Application.persistentDataPath + saveName;
            pathGeneral = Application.persistentDataPath + generalSaveName;
        #endif*/

        //File.Delete(pathGeneral);

        if (File.Exists(path)) {
            
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            savedGames = formatter.Deserialize(stream) as PlayerData[];

            stream.Close();

        } else {
            Debug.Log("Save file not found in " + path);
        }

        if (File.Exists(pathGeneral)) {

            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(pathGeneral, FileMode.Open);

            generalData = formatter.Deserialize(stream) as GameData;

            GameData.generalSave = generalData;

            stream.Close();

        } else {
            GameData.generalSave = new GameData();
            SaveGeneral();
        }

    }

    public static void Delete()
    {

        string path;
        path = Application.persistentDataPath + SaveSystem.saveName;

        /*#if UNITY_WEBGL
            path = System.IO.Path.Combine("/idbfs", Application.productName);
        #else
            path = Application.persistentDataPath + SaveSystem.saveName;
        #endif*/
        
        File.Delete(path);
       
        Debug.Log("Save successfully deleted!");
    }

    public static void EraseSave(int id)
    {
        savedGames[id] = null;

        BinaryFormatter formatter = new BinaryFormatter();

        string pathGeneral;
        string path;

        path = Application.persistentDataPath + SaveSystem.saveName;
        pathGeneral = Application.persistentDataPath + SaveSystem.generalSaveName;

        /*#if UNITY_WEBGL
            path = System.IO.Path.Combine("/idbfs", Application.productName);
        #else
            path = Application.persistentDataPath + SaveSystem.saveName;
            pathGeneral = Application.persistentDataPath + SaveSystem.generalSaveName;
        #endif*/

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, savedGames);

        stream.Close();

    }

    public static void EraseAllData()
    {

        generalData = null;

        BinaryFormatter formatter = new BinaryFormatter();

        string pathGeneral;
        pathGeneral = Application.persistentDataPath + SaveSystem.generalSaveName;

        /*#if UNITY_WEBGL
            pathGeneral = System.IO.Path.Combine("/idbfs", Application.productName);
        #else
            pathGeneral = Application.persistentDataPath + SaveSystem.generalSaveName;
        #endif*/

        FileStream stream = new FileStream(pathGeneral, FileMode.Create);

        formatter.Serialize(stream, generalData);

        stream.Close();

        for (int i = 0; i < 3; i++) {
            EraseSave(i);
        }
    }

}
