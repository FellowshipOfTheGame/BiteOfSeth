using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{

    public static PlayerData current;

    public int id;
    public int scene;
    public int score;
    public bool completedGame;
    public int totalScore;
    public int lorePieces;
    public int totalLorePieces;
    public float timer;
    public float totalTimer;
    
    public PlayerData(int _id)
    {
        id = _id;
        completedGame = false;
        scene = 1;
        score = totalScore = lorePieces = totalLorePieces = 0;
        timer = totalTimer = 0f;
    }
    
}
