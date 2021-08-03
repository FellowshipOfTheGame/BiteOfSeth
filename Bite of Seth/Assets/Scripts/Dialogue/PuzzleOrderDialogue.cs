using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOrderDialogue : DialogueBehavior {

    private bool selected = false, locked = false;

    public SpriteRenderer number;
    public Sprite[] digits;

    public PuzzleManager.Id id;
    public string statueName;

    // Update is called once per frame
    void Update()
    {
        //art.SetBool("hold", selected);
        if (ts.TryToDialogue()) {
            
        }
    }

    public override void OnDialog() {
        base.OnDialog();

        if (locked) return;
        //If the statue isnt selected yet, then select it
        if (!selected) {
            //TO DO: Ask the player is it really wants to select this statue
            //if(UI.ConfirmStatueSelection())
            int counter = ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().SelectStatue(id);
            if(counter > 0) {
                number.gameObject.SetActive(true);
                number.transform.eulerAngles = Vector3.zero;
                number.color = color;
                number.sprite = digits[counter - 1];
                selected = true;
            }
        } else {
            //If the statue is already selected, then unselect it
            number.gameObject.SetActive(false);
            ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().UnselectStatue(id);
            selected = false;
        }
    }

    //Reset statue selection
    public void ResetSelection() {
        selected = false;
        number.gameObject.SetActive(false);
    }

    public void SetId(PuzzleManager.Id _id)
    {
        id = _id;
    }

    public PuzzleManager.Id GetId()
    {
        return id;
    }

    public void UpdateCounter(int c) {
        number.sprite = digits[c - 1];
    }

    public void SetLock(bool value) {
        locked = value;
    }

}
