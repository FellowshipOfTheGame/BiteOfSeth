using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DialogueBehavior : MonoBehaviour {
    protected TextScript ts;
    public Animator art;

    protected Color color;

    public bool puzzleStatue = false;

    // Start is called before the first frame update
    void Start() {
        ts = GetComponentInChildren<TextScript>();
        ts.statue = this;
        color = ts.dialogueSequence[0].dialogueInfo[0].character.color;
    }

    // Update is called once per frame
    void Update() {
        if (ts.TryToDialogue() && puzzleStatue) {
            ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().AddTipStatue();
            puzzleStatue = false;
        }
    }

    public virtual void OnGetClose(){
        if (art != null) art.SetBool("player", true);
    }

    public virtual void OnGetAway() {
        if (art != null) art.SetBool("player", false);
    }

    public virtual void OnDialog(){
        if (art != null) art.SetBool("talk", true);
    }

    public virtual void OnEndDialog(){
        if (art != null) art.SetBool("talk", false);
    }
}
