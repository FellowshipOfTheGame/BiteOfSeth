﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScript : MonoBehaviour
{
    public DialogueBase dialogue;
    public bool playerInRange;

    public void TriggerDialogue(){
        DialogueManager.instance.EnqueueDialogue(dialogue);
    }

    public void ContinueDialogue(){
        DialogueManager.instance.DequeueDialogue();
    }

    public void UpdateLog(){
        LogManager.instance.AddEntry(dialogue);
    }

    void OnTriggerEnter2D(Collider2D other){
        //Debug.Log("Player entered trigger with " + other.tag);

        if(other.CompareTag("Player")){
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        //Debug.Log("Player has exit trigger with " + other.tag);

        if(other.CompareTag("Player")){
            playerInRange = false;
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
            //UpdateLog();
            //return true;
            return false;
        } else if (Input.GetKeyDown(KeyCode.E) && playerInRange && DialogueManager.instance.isDialogueActive == true) {
            ContinueDialogue();
            return true;
            //return false;
        }
        return false;

    }

}