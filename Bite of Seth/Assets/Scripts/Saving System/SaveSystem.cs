using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



public static class SaveSystem
{
    
    public static void SavePlayer(PlayerData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path;
        #if UNITY_WEBGL
            path = System.IO.Path.Combine("/idbfs", Application.productName);
        #else   
            path = Application.persistentDataPath + "/player.fun";
        #endif

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);

        stream.Close();

    }

    public static PlayerData LoadPlayer()
    {

        string path;
        #if UNITY_WEBGL
            path = System.IO.Path.Combine("/idbfs", Application.productName);
        #else
            path = Application.persistentDataPath + "/player.fun";
        #endif

        if (File.Exists(path)) {
            
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();

            return data;

        } else {
            Debug.LogError("Save file not found in " + path);
            return null;
        }

    }

    public static void DeletePlayer()
    {

        string path;
        #if UNITY_WEBGL
                path = System.IO.Path.Combine("/idbfs", Application.productName);
        #else
                path = Application.persistentDataPath + "/player.fun";
        #endif
        
        File.Delete(path);
       
        Debug.Log("Save successfully deleted!");
    }

}
