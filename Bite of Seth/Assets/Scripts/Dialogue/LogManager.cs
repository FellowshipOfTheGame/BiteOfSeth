﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour
{

    public static LogManager instance;
    private void Awake(){
        if(instance != null){
            Debug.LogError("Missing asset " + gameObject.name);
        } else {
            instance = this;
        }
    }

    public List<DialogueBase> dialogueLogs;
    public bool isDisplayingLogs = false;
    public GameObject logPrefab;
    public GameObject logsList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L)){
            toggleLogs();
        }
    }

    public void toggleLogs(){
        

        if(!isDisplayingLogs){
            //toggle on Logs UI
            logsList.SetActive(true);
            isDisplayingLogs = true;
        } else {
            //turn off Logs UI
            logsList.SetActive(false);
            isDisplayingLogs = false;
        }
    }

    public void AddEntry(DialogueBase db){
        if (!(dialogueLogs.Contains(db))){
            dialogueLogs.Add(db);

            foreach(DialogueBase.Info info in db.dialogueInfo){
                Debug.Log(info.myText);
                GameObject logsContent = GameObject.Find("LogsContent");
                GameObject newLog = Instantiate(logPrefab, logsList.transform.GetChild(0).transform.GetChild(0).transform);

                Text[] logInfo = (Text[]) newLog.GetComponentsInChildren<Text>(true);
                
                logInfo[0].text = info.character.characterName;
                logInfo[1].text = info.myText;
            }
            
            
        }
    }

    public void RemoveEntry(DialogueBase db){
        if(dialogueLogs.Contains(db)){
            dialogueLogs.Remove(db);
        }
    }
}
