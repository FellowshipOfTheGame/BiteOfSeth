﻿using System.Collections;
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
    public GameObject interactBox;
    public Text interactText;

    public float delay = 0.001f;

    public Queue<DialogueBase.Info> dialogueInfo = new Queue<DialogueBase.Info>();

    private bool isCurrentlyTyping;

    public bool isDialogueActive;
    private string completeText;

    private Coroutine inst;

    private string interactName;

    private AudioObject currentVoicedLine;
    private AudioSource dubAudSrc;

    public Color normalColor, highlightColor;

    public void EnqueueDialogue(DialogueBase db){
        dialogueInfo.Clear();
        dialogueBox.SetActive(true);
        isDialogueActive = true;
        GameManager gm = ServiceLocator.Get<GameManager>();
        gm.lockMovement++;
        //Debug.Log($"Encue lock++ ({gm.lockMovement})");
        foreach (DialogueBase.Info info in db.dialogueInfo){
            dialogueInfo.Enqueue(info);
        }

        DequeueDialogue();
    }

    public bool DequeueDialogue(){
        
        //Debug.Log("DequeueDialogue");
        DialogueBase.Info Info = null;

        //need to add code that detects when there is no more dialogue and return
        if(dialogueInfo.Count == 0 && isCurrentlyTyping){
            CompleteText();
            StopCoroutine(inst);
            isCurrentlyTyping = false;
            return false;
        } else if (dialogueInfo.Count == 0 && isCurrentlyTyping == false){
            EndOfDialogue();
            return true;
        }

        if(isCurrentlyTyping == true){
            CompleteText();
            StopCoroutine(inst);
            isCurrentlyTyping = false;
            return false;
        }

        //stop dub sound
        if (dubAudSrc != null)
            dubAudSrc.Stop();

        Info = dialogueInfo.Dequeue();

        string text = Info.myText;

        if (Info.needPuzzleInfo) {
            dialogueText.color = highlightColor;
            //Complete text with puzzle info
            string[] names = ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().GetStatuesNamesInOrder();
            //Replace the statues names in the text on the respectives <x> where x is the Id of the statue;
            for (int i = 0; i < names.Length; i++) {
                int fix = i + 1;
                text = text.Replace("<ID " + fix + ">", names[i]);
            }
            string ownName = Info.character.characterName;
            text = text.Replace(ownName + "'s", "my");
            //text = text.Replace(ownName, "my");
        } else {
            dialogueText.color = normalColor;
        }

        completeText = text;

        dialogueName.text = Info.character.characterName;
        dialogueText.text = text;
        dialoguePortrait.sprite = Info.character.portrait;

        //play dub sound
        currentVoicedLine = Info.voicedLine;
        if (currentVoicedLine != null)
            dubAudSrc = ServiceLocator.Get<AudioManager>().PlayAudio(currentVoicedLine);

        dialogueText.text = "";
        inst = StartCoroutine(TypeText(text));
        return false;
    }

    public void AbortDialogue(){
        if(dialogueInfo.Count == 0 && isCurrentlyTyping){
            CompleteText();
            StopCoroutine(inst);
            isCurrentlyTyping = false;
        } else if (dialogueInfo.Count == 0 && isCurrentlyTyping == false){
            EndOfDialogue();
        }

        if(isCurrentlyTyping == true){
            CompleteText();
            StopCoroutine(inst);
            isCurrentlyTyping = false;
        }

        dialogueInfo.Clear();
        dialogueBox.SetActive(false);
        isDialogueActive = false;
    }

    IEnumerator TypeText(string text)
    {
        isCurrentlyTyping = true;
        foreach (char c in text.ToCharArray()) {
            yield return new WaitForSeconds(delay);
            dialogueText.text += c;
        }
        isCurrentlyTyping = false;
    }

    private void CompleteText(){
        dialogueText.text = completeText;
    }

    public void EndOfDialogue(){
        if(isDialogueActive){
            //Debug.Log("End of Dialogue");
            isDialogueActive = false;
            dialogueBox.SetActive(false);
            GameManager gm = ServiceLocator.Get<GameManager>();
            gm.lockMovement--;

            //stop dub sound
            if (dubAudSrc != null)
                dubAudSrc.Stop();

            //Debug.Log($"End Dialogue lock-- ({gm.lockMovement})");
        }
    }

    public bool toggleInteractAlert(bool status){
        interactText.text = "Press E to interact with " + interactName;
        interactBox.SetActive(status);
        return status;
    }

    public void SetInteractName(string name)
    {
        interactName = name;
    }

}
