using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    public List<DialogueBase> dialogueSequence;
    private int index;

    [HideInInspector]
    public DialogueBehavior statue;

    private DialogueBase curDialogue;

    public bool playerInRange;
    public DoorTrigger doorTrigger = null;
    public bool isAutoTriggered;
    public bool repeatAutoTrigger;
    private PlayerController playerRef = null;

    bool talking = false;

    private void Start()
    {
        ResetDialogue();
    }

    public void TriggerDialogue(){
        statue.OnDialog();
        talking = true;
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
            statue.OnGetClose();

            if ((isAutoTriggered || repeatAutoTrigger) && !DialogueManager.instance.isDialogueActive) {
                isAutoTriggered = false;
                TriggerDialogue();
                UpdateLog();
            } else if (!DialogueManager.instance.isDialogueActive) {
                DialogueManager.instance.SetInteractName(curDialogue.dialogueInfo[0].character.characterName);
                DialogueManager.instance.toggleInteractAlert(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other){
        //Debug.Log("Player has exit trigger with " + other.tag);
        if(other.CompareTag("Player")){
            DialogueManager.instance.EndOfDialogue();
            playerInRange = false;
            statue.OnGetAway();

            if (DialogueManager.instance.isDialogueActive == false) {
                DialogueManager.instance.toggleInteractAlert(false);
            }
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

        if (Input.GetKeyDown(KeyCode.E) && playerInRange) {
            return Dialogue();
        }

        if (talking && !DialogueManager.instance.isDialogueActive) {
            statue.OnEndDialog();
            talking = false;
        }

        return false;

    }

    public bool Dialogue()
    {
        if (DialogueManager.instance.isDialogueActive == false) {
            TriggerDialogue();
            DialogueManager.instance.toggleInteractAlert(false);
            UpdateLog();
            return false;
        } else{
            bool dialogueEnded = ContinueDialogue();
            if (dialogueEnded) {
                if (doorTrigger != null) {
                    doorTrigger.SetState(true);
                }
                if (index >= dialogueSequence.Count) {
                    curDialogue = dialogueSequence[index - 1];
                } else {
                    curDialogue = dialogueSequence[index++];
                }
            }
            return true;
        }
    }    

    public PlayerController GetPlayerRef()
    {
        return playerRef;
    }

    public void ResetDialogue()
    {
        DialogueManager.instance.isDialogueActive = false;
        talking = false;
        index = 0;
        curDialogue = dialogueSequence[index++];
    }

    public void ChangeCurrentDialogueSequence(List<DialogueBase> dialogues)
    {
        dialogueSequence = dialogues;
        ResetDialogue();
    }

    public void SetDoorTrigger(DoorTrigger dt)
    {
        doorTrigger = dt;
    }

}