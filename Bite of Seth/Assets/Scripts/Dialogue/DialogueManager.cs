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

    public float delay = 0.001f;

    public Queue<DialogueBase.Info> dialogueInfo = new Queue<DialogueBase.Info>();

    private bool isCurrentlyTyping;

    public bool isDialogueActive;
    private string completeText;

    private Coroutine inst;
    
    public void EnqueueDialogue(DialogueBase db){
        dialogueInfo.Clear();
        dialogueBox.SetActive(true);
        isDialogueActive = true;

        foreach(DialogueBase.Info info in db.dialogueInfo){
            dialogueInfo.Enqueue(info);
        }

        DequeueDialogue();
    }

    public void DequeueDialogue(){

        DialogueBase.Info Info = null;

        //need to add code that detects when there is no more dialogue and return
        if(dialogueInfo.Count == 0 && isCurrentlyTyping){
            CompleteText();
            StopCoroutine(inst);
            isCurrentlyTyping = false;
            return;
        } else if (dialogueInfo.Count == 0 && isCurrentlyTyping == false){
            EndOfDialogue();
            return;
        }

        if(isCurrentlyTyping == true){
            CompleteText();
            StopCoroutine(inst);
            isCurrentlyTyping = false;
            return;
        }

        Info = dialogueInfo.Dequeue();
        completeText = Info.myText;

        dialogueName.text = Info.character.characterName;
        dialogueText.text = Info.myText;
        dialoguePortrait.sprite = Info.character.portrait;

        dialogueText.text = "";
        inst = StartCoroutine(TypeText(Info));

    }

    IEnumerator TypeText(DialogueBase.Info info){
        isCurrentlyTyping = true;
        foreach(char c in info.myText.ToCharArray()){
            yield return new WaitForSeconds(delay);
            dialogueText.text += c;
        }
        isCurrentlyTyping = false;
    }

    private void CompleteText(){
        dialogueText.text = completeText;
    }

    public void EndOfDialogue(){
        isDialogueActive = false;
        dialogueBox.SetActive(false);
    }

}
