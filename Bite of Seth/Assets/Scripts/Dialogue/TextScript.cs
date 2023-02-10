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

    private bool endOfConversation = false;

    private void Start()
    {
        ResetDialogue();
    }

    //Initiate the dialogue
    public void TriggerDialogue(){
        statue.OnDialog();
        talking = true;
        DialogueManager.instance.EnqueueDialogue(curDialogue);
        statue.OnUpdateDialog();
    }

    public bool ContinueDialogue(){
        bool dialogueEnded = DialogueManager.instance.DequeueDialogue();
        statue.OnUpdateDialog();
        return dialogueEnded;
    }

    public void UpdateLog(){
        //Update log with new entries
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
                DialogueManager.instance.SetInteractName(statue.character.characterName);
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

        if (Input.GetKeyDown(KeyCode.E) && playerInRange && !ServiceLocator.Get<GameManager>().pause) {
            return Dialogue();
        }

        if (talking && !DialogueManager.instance.isDialogueActive) {
            statue.OnEndDialog();
            talking = false;
            if (playerInRange && !endOfConversation) {
                DialogueManager.instance.toggleInteractAlert(true);
            }
        }

        return false;

    }

    public bool Dialogue()
    {
        //Return false if the dialogue just started or true if the first dialog ended already
        if (DialogueManager.instance.isDialogueActive == false) {
            //Start the dialogue
            TriggerDialogue();
            DialogueManager.instance.toggleInteractAlert(false);
            UpdateLog();
            return false;
        } else{
            //Try to continue dialoguing
            bool dialogueEnded = ContinueDialogue();
            if (dialogueEnded) {
                if (doorTrigger != null) {
                    doorTrigger.SetState(true);
                }
                if (index >= dialogueSequence.Count) {
                    curDialogue = dialogueSequence[index - 1];
                    endOfConversation = true;
                    Debug.Log("Fim do diálogo!");
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