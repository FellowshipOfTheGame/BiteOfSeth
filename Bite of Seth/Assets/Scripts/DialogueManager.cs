using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject player = null;
    public bool isInteracting = false;
    private int currentLine = 0;
    public DialogueObject currentDialogue;
    private bool dialogueAcquired = false;

    void Update(){
        
        if((player.GetComponent<DetectItem>()).item != null && dialogueAcquired == false){
            currentDialogue = (player.GetComponent<DetectItem>()).item.GetComponent<DialogueInteract>().dialogueObject;
            dialogueAcquired = true;
        } else if ((player.GetComponent<DetectItem>()).item == null && dialogueAcquired == true){
            currentDialogue = null;
            dialogueAcquired = false;
        }

        if(dialogueAcquired == true){
            if(Input.GetKeyDown(KeyCode.Space)){
                if(currentLine == currentDialogue.dialogueStrings.Count)
                    currentLine = 0;
                startDialogue();
            }
        }

    }

    private void startDialogue(){
        if(currentLine < currentDialogue.dialogueStrings.Count){
            Debug.Log(currentDialogue.dialogueStrings[currentLine]);
            currentLine++;
        }        
    }


}
