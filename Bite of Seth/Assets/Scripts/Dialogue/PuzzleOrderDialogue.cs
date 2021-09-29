using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleOrderDialogue : DialogueBehavior {

    private bool selected = false, locked = false;

    public SpriteRenderer number;
    [Space(5)]
    public Sprite digit_1;
    public Sprite digit_2;
    public Sprite digit_3;
    public Sprite digit_4;
    public Sprite digit_5;
    [Space(5)]
    public PuzzleManager.Id id;
    public string statueName;
    public UnityEvent OnSelectEvent, OnUnselectEvent;

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
                UpdateCounter(counter);
                selected = true;
            }
            OnSelectEvent.Invoke();
        } else {
            //If the statue is already selected, then unselect it
            number.gameObject.SetActive(false);
            ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().UnselectStatue(id);
            selected = false;
            OnUnselectEvent.Invoke();
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
        Debug.Log("Atualiza o numero da estatua "+ statueName + " para " + c);
        Debug.Log(number.sprite);
        number.gameObject.SetActive(false);
        switch (c) {
            case 1:
                number.sprite = digit_1;
                break;
            case 2:
                number.sprite = digit_2;
                break;
            case 3:
                number.sprite = digit_3;
                break;
            case 4:
                number.sprite = digit_4;
                break;
            case 5:
                number.sprite = digit_5;
                break;
        }
        number.gameObject.SetActive(true);
        Debug.Log(number.sprite);
    }

    public void SetLock(bool value) {
        locked = value;
    }

}
