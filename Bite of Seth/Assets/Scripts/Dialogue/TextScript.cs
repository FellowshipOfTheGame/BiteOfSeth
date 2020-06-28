using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    public DialogueBase dialogue;
    public bool playerInRange;
    public DoorTrigger doorTrigger = null;
    public bool isAutoTriggered;
    public bool repeatAutoTrigger;

    public void TriggerDialogue(){
        DialogueManager.instance.EnqueueDialogue(dialogue);
    }

    public bool ContinueDialogue(){
        bool dialogueEnded = DialogueManager.instance.DequeueDialogue();
        return dialogueEnded;
    }

    public void UpdateLog(){
        LogManager.instance.AddEntry(dialogue);
    }

    void OnTriggerEnter2D(Collider2D other){
        //Debug.Log("Player entered trigger with " + other.tag);
        if(other.CompareTag("Player")){
            playerInRange = true;
        }
        if((isAutoTriggered || repeatAutoTrigger) && !DialogueManager.instance.isDialogueActive){
            isAutoTriggered = false;
            TriggerDialogue();
            UpdateLog();
        } else if(!DialogueManager.instance.isDialogueActive){
            DialogueManager.instance.toggleInteractAlert(true);
        }
    }

    void OnTriggerExit2D(Collider2D other){
        //Debug.Log("Player has exit trigger with " + other.tag);
        DialogueManager.instance.AbortDialogue();
        if(other.CompareTag("Player")){
            playerInRange = false;
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
            //return true;
            return false;
        } else if (Input.GetKeyDown(KeyCode.E) && playerInRange && DialogueManager.instance.isDialogueActive == true) {
            bool dialogueEnded = ContinueDialogue();
            if (dialogueEnded && doorTrigger != null)
            {
                doorTrigger.SetState(true);
            }
            return true;
            //return false;
        }
        return false;

    }

}