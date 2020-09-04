using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName ="Manager/GameManager")]
public class GameManager : GameService
{
    public int score = 0;
    public int lockMovement = 0;
    public PlayerController player = null;

    private GameObject lm = null;
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

        score = 0;

        lockMovement = 0;

        levelData = new List<LevelData>();
        curLevelIndex = 0;
        
    }

    public void KillPlayer()
    {
        Debug.Log("TODO: KillPlayer Behavior");
        player.UseCheckpoint();
    }

    public void PrintTotalScore()
    {
        Debug.Log(string.Format("Current Score: {0}",score));
    }

    public void AddTotalScore(int value)
    {
        score += value;
    }

    public int GetTotalScore()
    {
        return score;
    }

    public void SetTotalScore(int value)
    {
        score = value;
    }

    public void TryToSetNewLevel()
    {
        //Try to find a LevelManager
        lm = GameObject.FindGameObjectWithTag("LevelManager");
        if(lm != null) {
            curLevel = lm.GetComponent<LevelManager>();
            Debug.Log("It's a Level Scene");
        } else {
            curLevel = null;
            Debug.Log("It's not a Level Scene");
        }
    }

    public void GoToNextLevel()
    {
        //Update with new values from finished level
        if(curLevel != null) UpdateLevelValues();
        ServiceLocator.Get<SceneReferences>().GoToNextScene();
    }

    public void FromLevelGoToScene(SceneReference scene)
    {
        //Update with new values from finished level
        if (curLevel != null) UpdateLevelValues();
        ServiceLocator.Get<SceneReferences>().GoToScene(scene);
    }

    //function to update Game Manager values with new values from Current Level
    public void UpdateLevelValues()
    {
        int score = GetLevelScore();
        AddTotalScore(score);
        PrintTotalScore();

        LevelData ld = new LevelData();
        ld.id = curLevelIndex++;
        ld.score = score;
        levelData.Add(ld);
    }

    public int GetLevelScore()
    {
        if(curLevel != null) {
            return curLevel.GetScore();
        }
        return 0;
    }

    public void AddLevelScore(int value)
    {
        if (curLevel != null) {
            curLevel.AddScore(value);
        }
    }

    public void PrintLevelScore()
    {
        if (curLevel != null) {
            curLevel.PrintScore();
        }
    }

    public void SetLevelScore(int value)
    {
        if (curLevel != null) {
            curLevel.SetScore(value);
        }
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