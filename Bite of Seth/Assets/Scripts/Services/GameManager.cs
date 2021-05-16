using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName ="Manager/GameManager")]
public class GameManager : GameService
{

    public int score;
    public int lorePieces;

    public int lockMovement = 0;
    public PlayerController player = null;

    private GameObject lm = null;
    private LevelManager curLevel;
    private int curLevelIndex;

    //Struct to store level data
    private struct LevelData {
        public int id;
        public int score;
    };

    private List<LevelData> levelData;

    private float timer = 0f;
    public bool timerTrigger = false;

    public override void Start()
    {
        base.Start();

        Cursor.visible = false;

        lockMovement = 0;

        score = lorePieces = 0;
        
        //Load the saved games
        SaveSystem.Load();

        levelData = new List<LevelData>();
        curLevelIndex = 0;
        
    }

    public override void Update()
    {
        base.Update();

        if (timerTrigger) {
            timer += Time.deltaTime;
        }

    }

    public int GetLevelDiamondsTotal()
    {
        int count = 0;
        List<GameObject> rooms = FindObjectOfType<TilemapSlicer>().GetRooms();
        foreach (GameObject room in rooms) {
            for (int i=0; i< room.transform.childCount; i++) {
                if (room.transform.GetChild(i).tag == "Diamond") {
                    count++;
                }
            }
        }
        return count;
    }

    public void StartNewGame(SceneReference scene)
    {
        score = lorePieces = 0;
        ServiceLocator.Get<SceneReferences>().GoToScene(scene);
    }

    public void DeleteSaves()
    {
        SaveSystem.Delete();
    }

    public SceneReference LoadGame()
    {
        score = PlayerData.current.score;
        lorePieces = PlayerData.current.lorePieces;
        return ServiceLocator.Get<SceneReferences>().GetSceneReference(PlayerData.current.scene);
    }

    public void SaveGame(SceneReference scene)
    {
        //SAVE GAME DATA
        int scene_index = ServiceLocator.Get<SceneReferences>().GetSceneIndex(scene);
        if(scene_index == 0) {
            //IF THE GAME IS COMPLETED
            Debug.Log("This is the endgame");

            PlayerData.current.completedGame = true;

            PlayerData.current.totalScore = GetTotalScore();
            Debug.Log("Total Score salvo: " + PlayerData.current.score);

            PlayerData.current.totalLorePieces = lorePieces;
            Debug.Log("Total Pieces of Lore salvo: " + PlayerData.current.totalLorePieces);

            PlayerData.current.totalTimer = PlayerData.current.timer + timer;
            Debug.Log("Total timer salvo: " + PlayerData.current.totalTimer);

            SaveSystem.Save();
        } else {
            //IF THE GAME IS NOT COMPLETED YET

            PlayerData.current.scene = scene_index;
            Debug.Log("Cena salva: " + PlayerData.current.scene);

            PlayerData.current.score = GetTotalScore();
            Debug.Log("Score salvo: " + PlayerData.current.score);

            PlayerData.current.lorePieces = lorePieces;
            Debug.Log("Pieces of Lore salvo: " + PlayerData.current.lorePieces);

            PlayerData.current.timer += timer;
            Debug.Log("Timer salvo: " + PlayerData.current.timer);

            SaveSystem.Save();
        }
    }

    public void KillPlayer()
    {
        Debug.Log("KillPlayer Behavior");
        player.Die();
    }

    public void PrintTotalScore()
    {
        Debug.Log(string.Format("Current Score: {0}", score));
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
        ResetTimer();
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

        SaveGame(scene);

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

    public void AddLorePiecesCount()
    {
        lorePieces += 1;
    }

    public void ResetTimer()
    {
        timerTrigger = true;
        timer = 0f;
    }

    public void StopTimer()
    {
        timerTrigger = false;
    }

}