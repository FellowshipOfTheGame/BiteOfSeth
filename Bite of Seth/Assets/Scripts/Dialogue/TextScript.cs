using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScript : MonoBehaviour
{
    public DialogueBase dialogue;
    public bool playerInRange;

    public void TriggerDialogue(){
        DialogueManager.instance.EnqueueDialogue(dialogue);
    }

    void OnTriggerEnter2D(Collider2D other){
        Debug.Log("Player entered trigger with " + other.tag);

        if(other.CompareTag("Player")){
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other){
        Debug.Log("Player has exit trigger with " + other.tag);

        if(other.CompareTag("Player")){
            playerInRange = false;
        }
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.E) && playerInRange){
            TriggerDialogue();
        }
    }

}
