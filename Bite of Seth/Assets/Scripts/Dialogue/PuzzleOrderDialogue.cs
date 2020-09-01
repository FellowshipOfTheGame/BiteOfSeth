using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOrderDialogue : DialogueBehavior
{

    private bool selected = false;

    private PuzzleManager.Id id;
    public string statueName;


    // Update is called once per frame
    void Update()
    {
        if (!selected) {
            if (ts.TryToDialogue())
            {
                //TO DO: Ask the player is it really wants to select this statue
                //if(UI.ConfirmStatueSelection())
                ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().SelectStatue(id);
                selected = true;
            }
        }
    }

    public void ResetSelection()
    {
        selected = false;
    }

    public void SetId(PuzzleManager.Id _id)
    {
        id = _id;
    }

}
