using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName ="Manager/GameManager")]
public class GameManager : GameService 
{ 

    public int lockMovement = 0;
    public PlayerController player = null;

    private GameObject lm = null;
    private LevelManager curLevel;
    private int curLevelIndex;

    private CameraFollow cf;
    public bool pause = false;

    public bool loadingNewScene = false;

    public int score;
    public int lorePieces;
    private float timer = 0f;
    public bool timerTrigger = false;
    public Vector2 savedSpawnPos = Vector2.zero;

    public override void Start()
    {
        base.Start();

        pause = false;

        Cursor.visible = false;

        lockMovement = 0;

        score = lorePieces = 0;

        savedSpawnPos = Vector2.zero;

        //Load the saved games
        SaveSystem.Load();

        ServiceLocator.Get<AudioManager>().SetMasterVolume(GameData.generalSave.generalVolume);
        ServiceLocator.Get<AudioManager>().SetBGMVolume(GameData.generalSave.BGMVolume);
        ServiceLocator.Get<AudioManager>().SetDialogueVolume(GameData.generalSave.dialogueVolume);

        timer = 0f;
        timerTrigger = false;

    }

    public override void Update()
    {
        base.Update();

        if (timerTrigger && curLevel) {
            AddLevelTimer(Time.deltaTime);
        }

    }

    public int GetLevelDiamondsTotal()
    {
        int count = 0;
        List<GameObject> rooms = FindObjectOfType<TilemapSlicer>().GetRooms();
        foreach (GameObject room in rooms) {
            for (int i=0; i< room.transform.childCount; i++) {
                if (room.transform.GetChild(i).tag == "Diamond" && room.transform.GetChild(i).GetComponent<CollectableBehavior>() && room.transform.GetChild(i).gameObject.activeInHierarchy) {
                    count++;
                }
            }
        }
        return count;
    }

    public int GetLevelLoreTotal()
    {
        SymbolBehavior[] lorePieces = FindObjectsOfType<SymbolBehavior>();
        return lorePieces.Length;
    }

    public void StartNewGame(SceneReference scene)
    {
        score = lorePieces = 0;
        timer = 0f;
        ServiceLocator.Get<SceneReferences>().GoToScene(scene);
    }

    public void DeleteSaves()
    {
        SaveSystem.Delete();
    }

    public void LoadGame()
    {
        score = PlayerData.current.totalScore;
        lorePieces = PlayerData.current.totalLorePieces;
        timer = PlayerData.current.totalTimer;
        ServiceLocator.Get<SceneReferences>().GoToSceneId(PlayerData.current.scene);
    }

    public void SaveGame(SceneReference scene)
    {
        ShowSavingWarning();
        
        //SAVE GAME DATA
        int scene_index = ServiceLocator.Get<SceneReferences>().GetSceneIndex(scene);
        if(scene_index == ServiceLocator.Get<SceneReferences>().scenesList.Count-1) {
            //IF THE GAME IS COMPLETED
            Debug.Log("This is the endgame");
            PlayerData.current.completedGame = true;          
        }

        PlayerData.current.scene = scene_index;
        Debug.Log("Cena salva: " + PlayerData.current.scene);

        PlayerData.current.totalScore = GetTotalScore();
        Debug.Log("Score salvo: " + PlayerData.current.totalScore);

        PlayerData.current.totalLorePieces = GetTotalPiecesOfLore();
        Debug.Log("Pieces of Lore salvo: " + PlayerData.current.totalLorePieces);

        PlayerData.current.totalTimer = GetTotalTimer();
        Debug.Log("Timer salvo: " + PlayerData.current.totalTimer);

        //Reset Level Data
        PlayerData.current.levelScore = 0;
        PlayerData.current.levelLorePieces = 0;
        PlayerData.current.levelTimer = 0f;

        SaveSystem.Save();
    }

    public void SaveSpawnPoint(Vector2 point)
    {
        ShowSavingWarning();
        savedSpawnPos = point;

        PlayerData.current.spawnPointX = savedSpawnPos.x;
        PlayerData.current.spawnPointY = savedSpawnPos.y;
        Debug.Log("Spawn Point salvo: ( " + PlayerData.current.spawnPointX + " , " + PlayerData.current.spawnPointY + " )");

        List<int> puzzleOrder = GetLevelPuzzleManager().GetStatuesOrder();
        int quantity = puzzleOrder.Count;

        if(quantity > 0) {
            PlayerData.current.statue0 = puzzleOrder[0];
            Debug.Log("Ordem de puzzle salvo, estatua 0: " + PlayerData.current.statue0);
        }

        if (quantity > 1) {
            PlayerData.current.statue1 = puzzleOrder[1];
            Debug.Log("Ordem de puzzle salvo, estatua 1: " + PlayerData.current.statue1);
        }

        if (quantity > 2) {
            PlayerData.current.statue2 = puzzleOrder[2];
            Debug.Log("Ordem de puzzle salvo, estatua 2: " + PlayerData.current.statue2);
        }

        if (quantity > 3) {
            PlayerData.current.statue3 = puzzleOrder[3];
            Debug.Log("Ordem de puzzle salvo, estatua 3: " + PlayerData.current.statue3);
        }

        if (quantity > 4) {
            PlayerData.current.statue4 = puzzleOrder[4];
            Debug.Log("Ordem de puzzle salvo, estatua 4: " + PlayerData.current.statue4);
        }

        int scene_index = ServiceLocator.Get<SceneReferences>().GetCurrentSceneIndex();
        PlayerData.current.scene = scene_index;
        Debug.Log("Cena salva: " + PlayerData.current.scene);

        PlayerData.current.levelScore = GetLevelScore();
        Debug.Log("Score salvo: " + PlayerData.current.levelScore);

        PlayerData.current.levelLorePieces = GetLevelPiecesOfLore();
        Debug.Log("Pieces of Lore salvo: " + PlayerData.current.levelLorePieces);

        PlayerData.current.levelTimer = GetLevelTimer();
        Debug.Log("Timer salvo: " + PlayerData.current.levelTimer);

        SaveSystem.Save();

    }

    //Return a boolean that indicates if there is a save point during the level
    public bool UpdateSavePointData()
    {
        SetLevelScore(PlayerData.current.levelScore);
        SetLevelPiecesOfLore(PlayerData.current.levelLorePieces);
        SetLevelTimer(PlayerData.current.levelTimer);
        savedSpawnPos.x = PlayerData.current.spawnPointX;
        savedSpawnPos.y = PlayerData.current.spawnPointY;
        return (savedSpawnPos.x != 0 || savedSpawnPos.y != 0);
    }

    public List<int> GetLoadedStatuesOrder()
    {
        List<int> puzzleOrder = new List<int>();

        if(PlayerData.current.statue0 != -1) {
            puzzleOrder.Add(PlayerData.current.statue0);
        }

        if (PlayerData.current.statue1 != -1) {
            puzzleOrder.Add(PlayerData.current.statue1);
        }

        if (PlayerData.current.statue2 != -1) {
            puzzleOrder.Add(PlayerData.current.statue2);
        }

        if (PlayerData.current.statue3 != -1) {
            puzzleOrder.Add(PlayerData.current.statue3);
        }

        if (PlayerData.current.statue4 != -1) {
            puzzleOrder.Add(PlayerData.current.statue4);
        }

        return puzzleOrder;

    }

    public void ShowSavingWarning()
    {
        SaveWarning sw = FindObjectOfType<SaveWarning>();
        if (sw) {
           sw.ShowSavingWarning();
        }
    }

    public void KillPlayer()
    {
        Debug.Log("KillPlayer Behavior");
        player.Die();
    }

    // PRIMEIRA FUNÇÃO CHAMADA QUANDO UMA CENA NOVA É CARREGADA
    // VERIFICA SE É UM LEVEL E INICIALIZA VALORES
    public void TryToSetNewLevel()
    {
        //Try to find a LevelManager
        lm = GameObject.FindGameObjectWithTag("LevelManager");
        if(lm != null) {
            curLevel = lm.GetComponent<LevelManager>();
            if (PlayerData.current != null) {
                SetLevelScore(PlayerData.current.levelScore);
                SetLevelPiecesOfLore(PlayerData.current.levelLorePieces);
                SetLevelTimer(PlayerData.current.levelTimer);
            }
            savedSpawnPos = Vector2.zero;
            timerTrigger = true;
            Debug.Log("It's a Level Scene");
        } else {
            curLevel = null;
            Debug.Log("It's not a Level Scene");
        }
    }

    public void GoToNextLevel()
    {
        //Update with new values from finished level
        if (curLevel != null) UpdateLevelValues();
        ServiceLocator.Get<SceneReferences>().GoToNextScene();
    }

    public void FromLevelGoToScene(SceneReference scene)
    {
        SaveSpawnPoint(Vector2.zero);

        //Update with new values from finished level
        if (curLevel != null) UpdateLevelValues();

        Debug.Log("SALVANDO");
        SaveGame(scene);

        ServiceLocator.Get<SceneReferences>().GoToScene(scene);
    }

    public void FromCutsceneGoToScene(SceneReference scene)
    {
        ServiceLocator.Get<SceneReferences>().GoToScene(scene);
    }

    //function to update Game Manager values with new values from Current Level
    public void UpdateLevelValues()
    {
        int score = GetLevelScore();
        int piecesOfLore = GetLevelPiecesOfLore();
        float timer = GetLevelTimer();
        AddTotalScore(score);
        AddTotalPiecesOfLore(piecesOfLore);
        AddTotalTimer(timer);
        PrintTotalScore();
        PrintTotalPiecesOfLore();
        PrintTotalTimer();
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

    public void StopTimer()
    {
        timerTrigger = false;
    }
    
    public void GetCameraRef()
    {
        cf = FindObjectOfType<CameraFollow>();
    }

    public void FocusCameraOnXDuringYSeconds(Vector3 x, float y)
    {
        cf.FocusCameraOnXDuringYSeconds(x, y);
    }

    public void changeCameraToCustomSize(float size)
    {
        cf.ChangeToCustomSize(size);
    }

    public void changeCameraToDefaultSize()
    {
        cf.ChangeToDefaultSize();
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

    public void PrintTotalPiecesOfLore()
    {
        Debug.Log(string.Format("Current Pieces of Lore: {0}", lorePieces));
    }

    public void AddTotalPiecesOfLore(int value)
    {
        lorePieces += value;
    }

    public int GetTotalPiecesOfLore()
    {
        return lorePieces;
    }

    public void SetTotalPiecesOfLore(int value)
    {
        lorePieces = value;
    }

    public void PrintTotalTimer()
    {
        Debug.Log(string.Format("Total Timer: {0}", timer));
    }

    public void AddTotalTimer(float value)
    {
        timer += value;
    }

    public float GetTotalTimer()
    {
        return timer;
    }

    public void SetTotalTimer(float value)
    {
        timer = value;
    }

    public int GetLevelScore()
    {
        if (curLevel != null) {
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

    public int GetLevelPiecesOfLore()
    {
        if (curLevel != null) {
            return curLevel.GetPiecesOfLore();
        }
        return 0;
    }

    public void AddLevelPiecesOfLore(int value)
    {
        if (curLevel != null) {
            curLevel.AddPiecesOfLore(value);
        }
    }

    public void PrintLevelPiecesOfLore()
    {
        if (curLevel != null) {
            curLevel.PrintPiecesOfLore();
        }
    }

    public void SetLevelPiecesOfLore(int value)
    {
        if (curLevel != null) {
            curLevel.SetPiecesOfLore(value);
        }
    }

    public float GetLevelTimer()
    {
        if (curLevel != null) {
            return curLevel.GetTimer();
        }
        return 0;
    }

    public void AddLevelTimer(float value)
    {
        if (curLevel != null) {
            curLevel.AddTimer(value);
        }
    }

    public void PrintLevelTimer()
    {
        if (curLevel != null) {
            curLevel.PrintTimer();
        }
    }

    public void SetLevelTimer(float value)
    {
        if (curLevel != null) {
            curLevel.SetTimer(value);
        }
    }

}