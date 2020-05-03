using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Manager/GameManager")]
public class GameManager : ScriptableObject
{
    private float score = 0;

    public void KillPlayer()
    {
        Debug.Log("TODO: KillPlayer Behavior");
    }

    public void PrintScore()
    {
        Debug.Log(string.Format("Current Score: {0}",score));
    }
    public void AddScore(float value)
    {
        score += value;
    }
}
