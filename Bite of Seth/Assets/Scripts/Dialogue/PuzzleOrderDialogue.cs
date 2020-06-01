using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOrderDialogue : MonoBehaviour
{

    TextScript ts;
    private bool selected = false;

    public PuzzleManager.Id id;

    // Start is called before the first frame update
    void Start()
    {
        ts = GetComponentInChildren<TextScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!selected) {
            if (ts.TryToDialogue())
            {
                //TO DO: Ask the player is it really wants to select this statue
                //if(UI.ConfirmStatueSelection())
                ServiceLocator.Get<PuzzleManager>().SelectStatue(id);
                selected = true;
            }
        }
    }

    public void ResetSelection()
    {
        selected = false;
    }

}
