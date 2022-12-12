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

    public bool[,] lorePiecesMap = new bool[3, 6];

    public float spawnPointX;
    public float spawnPointY;

    //In the future, if we need more than 5 statues, just add more
    public int statue0;
    public int statue1;
    public int statue2;
    public int statue3;
    public int statue4;

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
        statue0 = statue1 = statue2 = statue3 = statue4 = -1;
        for(int i=0; i<3; i++) {
            for (int j=0; j<6; j++) {
                lorePiecesMap[i, j] = false;
            }
        }
    }
    
}
