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

    public List<string> scenePathsList;
    private int curSceneIndex; 

    public override void Start()
    {
        base.Start();
        lockMovement = false;

        curSceneIndex = 0;
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
        //TODO: STORE OLD LEVELS
        //If there is a current level, destroy it and create another
        if (lm != null) Destroy(lm);
        lm = Instantiate(levelManagerPrefab);
        curLevel = lm.GetComponent<LevelManager>();
        curLevel.SetScenePath(scenePathsList[curSceneIndex]);
    }

    public void GoToNextLevel()
    {
        //Update with new values from finished level
        UpdateValues();
        curSceneIndex++;
        //Load new scene
        SceneManager.LoadScene(scenePathsList[curSceneIndex]);
        //Create a new Level
        SetNewLevel();
    }

    //function to update Game Manager values with new values from Current Level
    public void UpdateValues()
    {
        AddScore(GetLevelScore());
        PrintScore();
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
        //Load current scene again
        SceneManager.LoadScene(scenePathsList[curSceneIndex]);
        //Recreate the Level
        SetNewLevel();
        //TODO: RESTART PUZZLE
    }

}
