using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOrderDialogue : DialogueBehavior {

    private bool selected = false, locked = false;

    public SpriteRenderer number;
    public Sprite[] digits;

    private PuzzleManager.Id id;
    public string statueName;

    // Update is called once per frame
    void Update()
    {
        art.SetBool("hold", selected);
        if (ts.TryToDialogue()) {
            
        }
    }

    public override void OnDialog() {
        base.OnDialog();

        if (locked) return;

        if (!selected) {
            //TO DO: Ask the player is it really wants to select this statue
            //if(UI.ConfirmStatueSelection())
            number.gameObject.SetActive(true);
            number.transform.eulerAngles = Vector3.zero;
            number.color = color;
            int counter = ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().SelectStatue(id);
            number.sprite = digits[counter - 1];

            selected = true;
        } else {
            number.gameObject.SetActive(false);
            ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().UnselectStatue(id);

            selected = false;
        }
    }

    public void ResetSelection() {
        selected = false;
        number.gameObject.SetActive(false);
    }

    public void SetId(PuzzleManager.Id _id)
    {
        id = _id;
    }

    public void UpdateCounter(int c) {
        number.sprite = digits[c - 1];
    }

    public void SetLock(bool value) {
        locked = value;
    }

}
