using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneScript : MonoBehaviour
{
    public TimelineControl timeline;
    public List<DialogueBase> dialogueSequence;
    private int index;
    private DialogueBase curDialogue;
    bool talking = false, waiting = false, keepTalking = false, thenWait = false, emptyDialogue = false;
    public SceneReference NextLevelScene;

    private void Start() {
        DialogueManager.instance.isDialogueActive = false;
        talking = false;
        index = 0;
        curDialogue = dialogueSequence[index++];
    }

    public void TriggerDialogue(){
        if (emptyDialogue) {
            Debug.Log("No more dialogues on the list");
            return;
        }

        //Start Dialogue Chunk
        talking = true;
        OnDialogueStart();
        DialogueManager.instance.EnqueueDialogue(curDialogue);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (talking) {
                bool dialogueEnded = DialogueManager.instance.DequeueDialogue();

                if (dialogueEnded) {
                    if (index < dialogueSequence.Count) curDialogue = dialogueSequence[index++];
                    else emptyDialogue = true;

                    talking = false;
                    OnDialogueEnd();
                }
            
            }
        } else if (Input.GetKeyUp(KeyCode.Escape)) {
            EraseWarning();
            ExitCutscene();
        } else if (Input.anyKeyDown){
            DialogueManager.instance.toggleInteractWithCutsceneAlert(true);
            CancelInvoke();
            Invoke("EraseWarning", 1f);
        }
    }

    void EraseWarning() {
        DialogueManager.instance.toggleInteractWithCutsceneAlert(false);
    }



    
    void OnDialogueStart() {}

    void OnDialogueEnd() {
        if (waiting) {
            timeline.Resume();

            if (keepTalking) {
                TriggerDialogue();
                if (!thenWait) timeline.Resume();
                waiting = thenWait;
                keepTalking = false;
                thenWait = false;
            } else {
                waiting = false;
            }
            
        }
        
    }



    public void NextLine(bool wait) {
        if (!talking) {
            TriggerDialogue();
            if (wait) WaitForDialogue();
        } else {
            WaitForDialogue();
            thenWait = wait;
            keepTalking = true;
        }
    }

    public void WaitForDialogue() {
        if (talking) {
            timeline.Pause();
            waiting = true;
        }
    }

    public void ExitCutscene() {
        if (NextLevelScene != null) {
            ServiceLocator.Get<GameManager>().FromCutsceneGoToScene(NextLevelScene);
        } else {
            Debug.LogError("Sem referência para a próxima cena.");
        }
    }

}
