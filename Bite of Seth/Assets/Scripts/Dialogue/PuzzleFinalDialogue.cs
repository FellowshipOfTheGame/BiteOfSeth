using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleFinalDialogue : MonoBehaviour
{

    TextScript ts;
    public Animator art;

    // Start is called before the first frame update
    void Start()
    {
        ts = GetComponentInChildren<TextScript>();
        art.runtimeAnimatorController = ts.dialogue.dialogueInfo[0].character.art;
    }

    // Update is called once per frame
    void Update()
    {
        if(ts.TryToDialogue())
        {
            //TO DO: Ask the player is it really wants to give the final answer
            //if(UI.ConfirmFinalSelection())

            int resultado = ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().CheckFinalAnswer();

            switch (resultado) {

                case 0:
                    Debug.Log("CHOOSE ALL STATUES!");
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
                    ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().ResetPuzzle();
                    break;

            }
        }
    }

}
