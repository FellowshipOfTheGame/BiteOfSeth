using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Manager/GameManager")]
public class GameManager : GameService
{
    public int score = 0;
    public bool lockMovement = false;

    public override void Start()
    {
        base.Start();
        lockMovement = false;
    }

    public void KillPlayer()
    {
        Debug.Log("TODO: KillPlayer Behavior");
    }
    public void PrintScore()
    {
        Debug.Log(string.Format("Current Score: {0}",score));
    }
    public void AddScore(int value)
    {
        score += value;
    }
}
