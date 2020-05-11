using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleFinalDialogue : MonoBehaviour
{

    TextScript ts;
    private bool selected = false;

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
            if (PuzzleManager.instance.CheckFinalAnswer())
            {
                Debug.Log("YOU WON, CONGRATULATIONS!");
            } 
            else 
            {
                Debug.Log("YOU LOSE... TRY AGAIN!");
                PuzzleManager.instance.SetPuzzle();
            }
        }
    }

}
