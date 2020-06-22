using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int sceneBuildIndex;

    private int score;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSceneBuildIndex(int buildIndex)
    {
        sceneBuildIndex = buildIndex;
    }

    public void PrintScore()
    {
        Debug.Log(string.Format("Current Score: {0}", score));
    }

    public void AddScore(int value)
    {
        score += value;
    }

    public int GetScore()
    {
        return score;
    }
    
}
