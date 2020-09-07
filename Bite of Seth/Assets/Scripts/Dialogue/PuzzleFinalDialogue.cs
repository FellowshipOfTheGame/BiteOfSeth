using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleFinalDialogue : DialogueBehavior {

    public bool canDialogue = true;
    private bool firstTalk = true;
    private bool fail = false;
    private bool success = false;

    public List<DialogueBase> defaultDialogue;
    public List<DialogueBase> failDialogue;
    public List<DialogueBase> successDialogue;

    public DoorTrigger doorTrigger = null;

    public override void OnDialog(){
        base.OnDialog();

        if (!success) {
            if (firstTalk) {
                ts.ChangeCurrentDialogueSequence(defaultDialogue);
                firstTalk = false;
            }

            int resultado = ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().CheckFinalAnswer();

            switch (resultado) {
                case 1:
                    Debug.Log("YOU WON, CONGRATULATIONS!");
                    success = true;
                    ts.ChangeCurrentDialogueSequence(successDialogue);
                    ts.SetDoorTrigger(doorTrigger);
                    art.SetBool("hold", true);
                    ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().LockStatues();
                    break;

                case 2:
                    Debug.Log("YOU LOSE... TRY AGAIN!");
                    ts.ChangeCurrentDialogueSequence(failDialogue);
                    fail = true;
                    break;

            }
        }
    }

    public override void OnEndDialog() {
        base.OnEndDialog();

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
