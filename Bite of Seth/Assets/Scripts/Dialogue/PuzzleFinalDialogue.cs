using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleFinalDialogue : DialogueBehavior
{

    public GameObject warningPopup;
    public bool canDialogue = true;
    private bool firstTalk = true;
    private bool fail = false;
    private bool success = false;

    public List<DialogueBase> defaultDialogue;
    public List<DialogueBase> failDialogue;
    public List<DialogueBase> successDialogue;

    public DoorTrigger doorTrigger = null;

    // Update is called once per frame
    void Update()
    {

        if(canDialogue && ts.TryToDialogue())
        {
            //TO DO: Ask the player is it really wants to give the final answer
            //if(UI.ConfirmFinalSelection())

            if (fail) {
                if (ts.GetPlayerRef() != null) {
                    ServiceLocator.Get<GameManager>().KillPlayer();
                }
                ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().ResetChoices();
                ts.ChangeCurrentDialogueSequence(defaultDialogue);
                firstTalk = true;
                //ts.ResetDialogue();
                fail = false;
            }

            if (success) {
                return;
            }

            int resultado = ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().CheckFinalAnswer();

            switch (resultado) {

                case 0:
                    Debug.Log("CHOOSE ALL STATUES!");
                    if (firstTalk) {
                        firstTalk = false;
                    } else {
                        TriggerWarningAlert();
                    }
                    break;

                case 1:
                    Debug.Log("YOU WON, CONGRATULATIONS!");
                    success = true;
                    ts.ChangeCurrentDialogueSequence(successDialogue);
                    ts.SetDoorTrigger(doorTrigger);
                    ts.Dialogue();
                    break;

                case 2:
                    Debug.Log("YOU LOSE... TRY AGAIN!");
                    ts.ChangeCurrentDialogueSequence(failDialogue);
                    ts.Dialogue();
                    fail = true;
                    break;

            }
        }
    }

    private void TriggerWarningAlert()
    {
        Debug.Log("TriggerWarningAlert");
        ServiceLocator.Get<GameManager>().lockMovement += 1;
        warningPopup.SetActive(true);
        canDialogue = false;
    }

}
