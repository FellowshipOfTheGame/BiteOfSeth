using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager instance;
    private void Awake(){
        if(instance != null){
            Debug.LogError("Missing asset " + gameObject.name);
        } else {
            instance = this;
        }
    }

    public GameObject dialogueBox;
    public Text dialogueName;
    public Text dialogueText;
    public Image dialoguePortrait;

    public Queue<DialogueBase.Info> dialogueInfo = new Queue<DialogueBase.Info>();
    
    public void EnqueueDialogue(DialogueBase db){
        dialogueInfo.Clear();
        dialogueBox.SetActive(true);

        foreach(DialogueBase.Info info in db.dialogueInfo){
            dialogueInfo.Enqueue(info);
        }

        DequeueDialogue();
    }

    public void DequeueDialogue(){
        //need to add code that detects when there is no more dialogue and return
        if(dialogueInfo.Count == 0){
            endOfDialogue();
            return;
        }


        DialogueBase.Info Info = dialogueInfo.Dequeue();

        dialogueName.text = Info.characterName;
        dialogueText.text = Info.myText;
        dialoguePortrait.sprite = Info.portrait;
    }

    public void endOfDialogue(){
        dialogueBox.SetActive(false);
    }

}
