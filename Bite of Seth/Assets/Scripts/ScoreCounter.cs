using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    private int currentPoints = 0;

    public void AddPoints(int points)
    {
        currentPoints += points;
        Debug.Log(currentPoints);
    }
}
