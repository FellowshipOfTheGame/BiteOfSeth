using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int scene;
    public int score;
    //int piecesOfLore;
    //float time;

    //records...

    public PlayerData(int _scene, int _score)
    {
        scene = _scene;
        score = _score;
    }
    
}
