using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    public DialogueBase dialogue;
    public DialogueBase repetitiveDialogue;
    private DialogueBase curDialogue;

    public bool playerInRange;
    public DoorTrigger doorTrigger = null;
    public bool isAutoTriggered;
    public bool repeatAutoTrigger;
    private PlayerController playerRef = null;

    [HideInInspector] public DialogueBehavior statue;

    private void Start()
    {
        curDialogue = dialogue;
    }

    public void TriggerDialogue(){
        statue.OnDialog();
        DialogueManager.instance.EnqueueDialogue(curDialogue);
    }

    public bool ContinueDialogue(){
        bool dialogueEnded = DialogueManager.instance.DequeueDialogue();
        return dialogueEnded;
    }

    public void UpdateLog(){
        LogManager.instance.AddEntry(curDialogue);
    }

    void OnTriggerEnter2D(Collider2D other){
        //Debug.Log("Player entered trigger with " + other.tag);
        if(other.CompareTag("Player")){
            playerInRange = true;
            playerRef = other.gameObject.GetComponent<PlayerController>();
            statue.OnEnterDialog();
        }
        if((isAutoTriggered || repeatAutoTrigger) && !DialogueManager.instance.isDialogueActive){
            isAutoTriggered = false;
            TriggerDialogue();
            UpdateLog();
        } else if(!DialogueManager.instance.isDialogueActive){
            DialogueManager.instance.SetInteractName(curDialogue.dialogueInfo[0].character.characterName);
            DialogueManager.instance.toggleInteractAlert(true);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        //Debug.Log("Player has exit trigger with " + other.tag);
        DialogueManager.instance.AbortDialogue();
        if(other.CompareTag("Player")){
            playerInRange = false;
            statue.OnEndDialog();
        }
        if(DialogueManager.instance.isDialogueActive == false){
            DialogueManager.instance.toggleInteractAlert(false);
        }

    }

    // void Update(){
    //     if(Input.GetKeyDown(KeyCode.E) && playerInRange){
    //         TriggerDialogue();
    //         UpdateLog();
    //     }
    // }

    //Return true if the dialog has occurred
    public bool TryToDialogue() { 

        if (Input.GetKeyDown(KeyCode.E) && playerInRange && DialogueManager.instance.isDialogueActive == false) {
            TriggerDialogue();
            DialogueManager.instance.toggleInteractAlert(false);
            UpdateLog();
            return false;
        } else if (Input.GetKeyDown(KeyCode.E) && playerInRange && DialogueManager.instance.isDialogueActive == true) {
            bool dialogueEnded = ContinueDialogue();
            if (dialogueEnded) {
                if (doorTrigger != null) {
                    doorTrigger.SetState(true);
                }
                if (repetitiveDialogue != null) {
                    curDialogue = repetitiveDialogue;
                }
            }
            return true;
        }
        return false;

    }

    public PlayerController GetPlayerRef()
    {
        return playerRef;
    }

}