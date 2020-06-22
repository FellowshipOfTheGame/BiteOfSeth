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
 
    private int curSceneIndex; 

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

        curSceneIndex = SceneManager.GetActiveScene().buildIndex;
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
        curLevel.SetSceneBuildIndex(curSceneIndex);
    }

    public void GoToNextLevel()
    {

        //Update with new values from finished level
        UpdateValues();
        curSceneIndex++;

        //Debug.Log("Active Scene : " + SceneManager.GetActiveScene().path);
        //Load new scene
        SceneManager.LoadScene(curSceneIndex, LoadSceneMode.Single);

    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("New Scene Loaded");
        SceneManager.SetActiveScene(scene);
        Debug.Log("Active Scene : " + SceneManager.GetActiveScene().path);

        //Create a new Level
        SetNewLevel();

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
        //Load current scene again
        SceneManager.LoadScene(curSceneIndex, LoadSceneMode.Single);

        //Recreate the Level
        SetNewLevel();
    }

    public PuzzleManager GetLevelPuzzleManager()
    {
        return lm.GetComponent<PuzzleManager>();
    }

}