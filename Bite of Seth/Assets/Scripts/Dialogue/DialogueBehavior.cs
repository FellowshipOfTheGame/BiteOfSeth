﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBehavior : MonoBehaviour {
    protected TextScript ts;
    public Animator art;

    public bool puzzleStatue = false;

    // Start is called before the first frame update
    void Start() {
        ts = GetComponentInChildren<TextScript>();
        ts.statue = this;
        art.runtimeAnimatorController = ts.dialogueSequence[0].dialogueInfo[0].character.art;
    }

    // Update is called once per frame
    void Update() {
        if (ts.TryToDialogue() && puzzleStatue) {
            ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().AddTipStatue();
            puzzleStatue = false;
        }
    }

    public void OnGetClose(){
        art.SetBool("player", true);
    }

    public void OnGetAway() {
        art.SetBool("player", false);
    }

    public void OnDialog(){
        art.SetBool("talk", true);
    }

    public void OnEndDialog(){
        art.SetBool("talk", false);
    }
}
