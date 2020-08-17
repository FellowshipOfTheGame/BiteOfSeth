using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleFinalDialogue : MonoBehaviour
{

    TextScript ts;
    public Animator art;
    public GameObject warningPopup;
    public bool canDialogue = true;
    private bool firstTalk = true;

    // Start is called before the first frame update
    void Start()
    {
        ts = GetComponentInChildren<TextScript>();
        art.runtimeAnimatorController = ts.dialogueSequence[0].dialogueInfo[0].character.art;
    }

    // Update is called once per frame
    void Update()
    {

        if(canDialogue && ts.TryToDialogue())
        {
            //TO DO: Ask the player is it really wants to give the final answer
            //if(UI.ConfirmFinalSelection())

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
                    ServiceLocator.Get<GameManager>().GoToNextLevel();
                    break;

                case 2:
                    Debug.Log("YOU LOSE... TRY AGAIN!");
                    if (ts.GetPlayerRef() != null) {
                        ServiceLocator.Get<GameManager>().KillPlayer();
                    }
                    ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().ResetChoices();
                    firstTalk = true;
                    ts.ResetDialogue();
                    break;

            }
        }
    }

    private void TriggerWarningAlert()
    {
        ServiceLocator.Get<GameManager>().lockMovement = true;
        warningPopup.SetActive(true);
        canDialogue = false;
    }

}
