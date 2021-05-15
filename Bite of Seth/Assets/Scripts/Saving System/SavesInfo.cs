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
        public Text level;
    }

    public static int nSaves = 3;
    public SaveInfo[] saves = new SaveInfo[nSaves];

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<nSaves; i++) {
            if (SaveSystem.savedGames[i] != null) {
                saves[i].noSave.SetActive(true);
                saves[i].info.SetActive(true);
                saves[i].diamondsCount.text = SaveSystem.savedGames[i].totalScore.ToString() + " / 200";
                saves[i].level.text = "Level " + SaveSystem.savedGames[i].scene.ToString();
            }
        }   
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
