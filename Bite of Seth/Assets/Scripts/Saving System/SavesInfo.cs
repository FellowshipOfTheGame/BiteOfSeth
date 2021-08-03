using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SavesInfo : MonoBehaviour
{
    [System.Serializable]
    public struct SaveInfo {
        public GameObject info;
        public GameObject noSave;
        public Text diamondsCount;
        public Text loreCount;
        public Text level;
        public Text time;
    }

    public static int nSaves = 3;
    public SaveInfo[] saves = new SaveInfo[nSaves];

    public SceneReference firstScene;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<nSaves; i++) {
            if (SaveSystem.savedGames[i] != null) {
                saves[i].noSave.SetActive(false);
                saves[i].info.SetActive(true);
                PlayerData data = SaveSystem.savedGames[i];
                saves[i].diamondsCount.text = (data.totalScore + data.levelScore).ToString() + " / 200";
                saves[i].loreCount.text = (data.totalLorePieces + data.levelLorePieces).ToString() + " / 10";
                var ts = TimeSpan.FromSeconds(data.totalTimer + data.levelTimer);
                saves[i].time.text = "Time: " + string.Format("{0:00}:{1:00}", ts.TotalMinutes, ts.Seconds) + " min";
                if (data.completedGame) {
                    saves[i].level.text = "Completed";
                } else {
                    saves[i].level.text = ServiceLocator.Get<SceneReferences>().GetLevelName(data.scene);
                }
            }
        }   
    }

    // CREATE A NEW GAME
    public void ChooseSave(int id)
    {
        if (id < 0 || id >= SaveSystem.savedGames.Length) {
            Debug.Log("Invalid Save ID choosed");
            return;
        }
        SaveSystem.EraseSave(id);
        PlayerData.current = new PlayerData(id);
        ServiceLocator.Get<GameManager>().StartNewGame(firstScene);
    }

    // LOAD GAME
    public void LoadSave(int id)
    {
        if (id < 0 || id >= SaveSystem.savedGames.Length) {
            Debug.Log("Invalid Save ID loaded");
            return;
        }
        PlayerData.current = SaveSystem.savedGames[id];
        if(PlayerData.current == null) {
            return;
        }
        ServiceLocator.Get<GameManager>().LoadGame();
    }

}
