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

    //Struct to store level data
    private struct LevelData {
        public int id;
        public int score;
    };

    private List<LevelData> levelData;
    private PlayerData saveData;
    private bool hasSaveFile = false;

    public override void Start()
    {
        base.Start();

        Cursor.visible = false;

        score = 0;

        lockMovement = 0;
        
        //SaveSystem.DeletePlayer();
        saveData = SaveSystem.LoadPlayer();
        if (saveData == null) {
            //if there is no save file, then create one
            saveData = new PlayerData(1, 0);
            hasSaveFile = false;
        } else {
            hasSaveFile = true;
        }

        levelData = new List<LevelData>();
        curLevelIndex = 0;
        
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
        score = 0;
        ServiceLocator.Get<SceneReferences>().GoToScene(scene);
    }

    public void DeleteSaveFile()
    {
        SaveSystem.DeletePlayer();
        hasSaveFile = false;
    }

    public bool HasSaveFile()
    {
        return hasSaveFile;
    }

    public SceneReference LoadGame()
    {
        score = saveData.score;
        return ServiceLocator.Get<SceneReferences>().GetSceneReference(saveData.scene);
    }

    public void SaveGame(SceneReference scene)
    {
        //SAVE GAME DATA
        int scene_index = ServiceLocator.Get<SceneReferences>().GetSceneIndex(scene);
        if(scene_index == 0) {
            //IF THE GAME ENDED, DONT SAVE
            Debug.Log("This is the endgame, lol");
        } else {
            saveData.scene = scene_index;
            Debug.Log("Cena salva: " + saveData.scene);
            saveData.score = GetTotalScore();
            Debug.Log("Score salvo: " + saveData.score);
            SaveSystem.SavePlayer(saveData);
        }
        hasSaveFile = true;
    }

    public void KillPlayer()
    {
        Debug.Log("KillPlayer Behavior");
        player.Die();
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

}