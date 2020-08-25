using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBehavior : MonoBehaviour
{
    TextScript ts;
    public Animator art;

    // Start is called before the first frame update
    void Start()
    {
        ts = GetComponentInChildren<TextScript>();
        ts.statue = this;
        art.runtimeAnimatorController = ts.dialogueSequence[0].dialogueInfo[0].character.art;
    }

    // Update is called once per frame
    void Update()
    {
        ts.TryToDialogue();
    }

    public void OnEnterDialog(){
        art.SetBool("player", true);
    }

    public void OnDialog(){
        art.SetTrigger("talk");
    }

    public void OnEndDialog(){
        art.SetBool("player", false);
    }
}
