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
    //int piecesOfLore;
    //float time;

    //records...

    public PlayerData(int _id, int _scene, int _score)
    {
        id = _id;
        scene = _scene;
        score = _score;
        completedGame = false;
        totalScore = _score;
    }
    
}
