using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PuzzleFinalDialogue : DialogueBehavior {

    public Animator art;
    public bool selectAllStatues = true;
    public int nSelections = 1;

    public bool canDialogue = true;
    private bool firstTalk = true;
    private bool fail = false;
    private bool success = false;

    [SerializeField] AudioObject winChime = null;

    public List<DialogueBase> defaultDialogue;
    public List<DialogueBase> failDialogue;
    public List<DialogueBase> successDialogue;

    public DoorTrigger doorTrigger = null;

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

            //If its the first dialogue, set the default one 
            if (firstTalk) {
                ts.ChangeCurrentDialogueSequence(defaultDialogue);
                firstTalk = false;
            }

            //get the players answer result
            int result = ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().CheckFinalAnswer();

            switch (result) {
                //If the answer was right
                case 1:
                    Debug.Log("YOU WON, CONGRATULATIONS!");
                    success = true;
                    //Change dialogue to the success one
                    ts.ChangeCurrentDialogueSequence(successDialogue);
                    //ts.SetDoorTrigger(doorTrigger);
                    art.SetBool("hold", true);
                    //Lock dialogue with all puzzle statues
                    ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().LockStatues();
                    DialogueEndEvent.AddListener(() => {OnSuccessEvent.Invoke();} );
                    //OnSuccessEvent;

                    if(winChime)
                    {
                        ServiceLocator.Get<AudioManager>().PlayAudio(winChime);
                    }
                    else
                    {
                        Debug.LogError("Win Chime shouldn't be null");
                    }
                    break;
                //If the answer was wrong
                case 2:
                    Debug.Log("YOU LOSE... TRY AGAIN!");
                    //Change dialogue to the fail one
                    ts.ChangeCurrentDialogueSequence(failDialogue);
                    fail = true;
                    break;

            }
        }
    }

    public override void OnEndDialog() {
        base.OnEndDialog();

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

}
