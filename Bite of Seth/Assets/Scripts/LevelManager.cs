﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    private int score;
    private int piecesOfLore;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        piecesOfLore = 0;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void SetScore(int value)
    {
        score = value;
    }

    public void PrintPiecesOfLore()
    {
        Debug.Log(string.Format("Current Lore Pieces count: {0}", piecesOfLore));
    }

    public void AddPiecesOfLore(int value)
    {
        piecesOfLore += value;
    }

    public int GetPiecesOfLore()
    {
        return piecesOfLore;
    }

    public void SetPiecesOfLore(int value)
    {
        piecesOfLore = value;
    }

    public void PrintTimer()
    {
        Debug.Log(string.Format("Level Timer: {0}", timer));
    }

    public void AddTimer(float value)
    {
        timer += value;
    }

    public float GetTimer()
    {
        return timer;
    }

    public void SetTimer(float value)
    {
        timer = value;
    }

}
