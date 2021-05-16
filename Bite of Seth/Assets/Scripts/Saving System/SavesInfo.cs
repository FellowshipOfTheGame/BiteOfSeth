using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                saves[i].diamondsCount.text = SaveSystem.savedGames[i].totalScore.ToString() + " / 200";
                saves[i].loreCount.text = SaveSystem.savedGames[i].totalLorePieces.ToString() + " / 10";
                saves[i].level.text = ServiceLocator.Get<SceneReferences>().GetLevelName(SaveSystem.savedGames[i].scene);
                saves[i].time.text = "Time: " + ((int)SaveSystem.savedGames[i].timer / 60).ToString() + ":" + ((int)SaveSystem.savedGames[i].timer % 60).ToString();
            }
        }   
    }

    public void ChooseSave(int id)
    {
        if (id < 0 || id >= SaveSystem.savedGames.Length) {
            Debug.Log("Invalid Save ID choosed");
            return;
        }
        PlayerData.current = new PlayerData(id);
        ServiceLocator.Get<GameManager>().StartNewGame(firstScene);
    }

}
