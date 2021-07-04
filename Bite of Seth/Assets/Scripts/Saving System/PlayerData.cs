using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

    public static PlayerData current;

    public int id;
    public int scene;
    public bool completedGame;

    public int levelScore;
    public int levelLorePieces;
    public float levelTimer;

    public int totalScore;
    public int totalLorePieces;
    public float totalTimer;

    public float spawnPointX;
    public float spawnPointY;

    // Settings info: only matters is the PlayerData with id 0
    public float generalVolume;
    public float BGMVolume;
    public float dialogueVolume;

    public PlayerData(int _id)
    {
        id = _id;
        completedGame = false;
        scene = 1;
        levelScore = totalScore = levelLorePieces = totalLorePieces = 0;
        levelTimer = totalTimer = spawnPointX = spawnPointY = 0f;
        generalVolume = BGMVolume = dialogueVolume = 1f;
    }
    
}
