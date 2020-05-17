using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.L)){
        // }
    }

    public void toggleLogs(){
        if(!isDisplayingLogs){
            //toggle on Logs UI
            
            isDisplayingLogs = true;
        } else {
            //turn off Logs UI
            
            isDisplayingLogs = false;
        }
    }

    public void AddEntry(DialogueBase db){
        if (!(dialogueLogs.Contains(db))){
            dialogueLogs.Add(db);
        }
    }

    public void RemoveEntry(DialogueBase db){
        if(dialogueLogs.Contains(db)){
            dialogueLogs.Remove(db);
        }
    }
}
