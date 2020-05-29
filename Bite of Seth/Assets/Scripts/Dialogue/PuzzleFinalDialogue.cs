using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleFinalDialogue : MonoBehaviour
{

    TextScript ts;

    // Start is called before the first frame update
    void Start()
    {
        ts = GetComponentInChildren<TextScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(ts.TryToDialogue())
        {
            //TO DO: Ask the player is it really wants to give the final answer
            //if(UI.ConfirmFinalSelection())
            
            if (ServiceLocator.Get<PuzzleManager>().CheckFinalAnswer()) 
            {
                Debug.Log("YOU WON, CONGRATULATIONS!");
                ServiceLocator.Get<PuzzleManager>().ResetPuzzle();
            } 
            else 
            {
                Debug.Log("YOU LOSE... TRY AGAIN!");
                ServiceLocator.Get<PuzzleManager>().ResetPuzzle();
            }
        }
    }

}
