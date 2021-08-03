using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{

    // General purpose saving
    public static GameData generalSave;

    public float generalVolume;
    public float BGMVolume;
    public float dialogueVolume;
    
    public GameData()
    {
        generalVolume = BGMVolume = dialogueVolume = 1f;
    }
    
}
