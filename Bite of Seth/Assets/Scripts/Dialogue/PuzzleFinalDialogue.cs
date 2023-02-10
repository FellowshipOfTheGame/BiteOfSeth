using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PuzzleFinalDialogue : DialogueBehavior {

    public Animator art;
    public bool selectAllStatues = true;
    public int nSelections = 1;
    public bool isAnubisPuzzle  = false;

    public bool canDialogue = true;
    private bool firstTalk = true;
    private bool fail = false;
    private bool success = false;

    [SerializeField] AudioObject winChime = null;

    public List<DialogueBase> defaultDialogue;
    public List<DialogueBase> failDialogue;
    public List<DialogueBase> successDialogue;

    public DoorTrigger doorTrigger = null;

    public UnityEvent OnQuestionEvent;

    public UnityEvent OnAnswerEvent;

    public UnityEvent OnSuccessEvent;

    public bool IsAllStatuesSelectable()
    {
        return selectAllStatues;
    }

    public int GetPuzzleTotalSelectionsQuantity()
    {
        return nSelections;
    }

    public override void OnDialog(){
        base.OnDialog();
        
        //if the player didnt win yet
        if (!success) {

            

            //get the players answer result
            int result = ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().CheckFinalAnswer();

            switch (result) {
                //If the answer was right
                case 1:
                    Debug.Log("YOU WON, CONGRATULATIONS!");
                    OnAnswerEvent.Invoke();
                    success = true;
                    //Change dialogue to the success one
                    ts.ChangeCurrentDialogueSequence(successDialogue);
                    art.SetBool("hold", true);
                    //Lock dialogue with all puzzle statues
                    ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().LockStatues();
                    break;
                //If the answer was wrong
                case 2:
                    Debug.Log("YOU LOSE... TRY AGAIN!");
                    OnAnswerEvent.Invoke();
                    //Change dialogue to the fail one
                    ts.ChangeCurrentDialogueSequence(failDialogue);
                    fail = true;
                    break;

            }
        }
    }

    public override void OnEndDialog() {
        base.OnEndDialog();

        //If its the first dialogue, set the default one 
        if (firstTalk) {
            ts.ChangeCurrentDialogueSequence(defaultDialogue);
            OnQuestionEvent.Invoke();
            firstTalk = false;
        }

        //If the player succeed in the puzzle then play the chime and call the event
        if (success) {
            if (winChime) {
                ServiceLocator.Get<AudioManager>().PlayAudio(winChime);
            }
            else {
                Debug.LogError("Win Chime shouldn't be null");
            }

            OnSuccessEvent.Invoke();
        }

        //If the player failed in the puzzle then it dies and the choices are reseted
        if (fail) {
            if (ts.GetPlayerRef() != null) {
                ServiceLocator.Get<GameManager>().KillPlayer();
            }
            ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().ResetChoices();
            firstTalk = true;
            fail = false;
        }
    }

    void OnAskEnigma() {
        OnQuestionEvent.Invoke();
        DialogueEndEvent.RemoveListener(() => { OnAskEnigma(); });
    }

    void OnRightAnswer() {
        OnSuccessEvent.Invoke();
        DialogueEndEvent.RemoveListener(() => { OnRightAnswer(); });
    }
}
