using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName ="Manager/GameManager")]
public class GameManager : GameService
{
    public int score = 0;
    public bool lockMovement = false;

    public GameObject levelManagerPrefab;
    private GameObject lm;
    private LevelManager curLevel;
    private int curLevelIndex;

    private struct LevelData {
        public int id;
        public int score;
    };

    private List<LevelData> levelData;

    public override void Start()
    {
        base.Start();
        lockMovement = false;

        levelData = new List<LevelData>();
        curLevelIndex = 0;
        
        SetNewLevel();
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

    public void SetNewLevel()
    {
        //If there is a current level, destroy it and create another
        if (lm != null) Destroy(lm);
        lm = Instantiate(levelManagerPrefab);
        curLevel = lm.GetComponent<LevelManager>();
    }

    public void GoToNextLevel()
    {
        //Update with new values from finished level
        UpdateValues();
        ServiceLocator.Get<SceneReferences>().GoToNextScene();
    }

    //function to update Game Manager values with new values from Current Level
    public void UpdateValues()
    {
        int score = GetLevelScore();
        AddScore(score);
        PrintScore();

        LevelData ld = new LevelData();
        ld.id = curLevelIndex++;
        ld.score = score;
        levelData.Add(ld);
    }

    public int GetLevelScore()
    {
        return curLevel.GetScore();
    }

    public void AddLevelScore(int value)
    {
        curLevel.AddScore(value);
    }

    public void PrintLevelScore()
    {
        curLevel.PrintScore();
    }

    public void RestartLevel()
    {
        ServiceLocator.Get<SceneReferences>().ReloadCurrentScene();
    }

    public PuzzleManager GetLevelPuzzleManager()
    {
        return lm.GetComponent<PuzzleManager>();
    }

}