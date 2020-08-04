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
        art.runtimeAnimatorController = ts.dialogue.dialogueInfo[0].character.art;
    }

    // Update is called once per frame
    void Update()
    {
        ts.TryToDialogue();
    }
}
