using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOrderDialogue : MonoBehaviour
{

    TextScript ts;
    private bool selected = false;

    private PuzzleManager.Id id;
    public string statueName;
    public Animator art;

    // Start is called before the first frame update
    void Start()
    {
        ts = GetComponentInChildren<TextScript>();
        art.runtimeAnimatorController = ts.dialogueSequence[0].dialogueInfo[0].character.art;
    }

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
