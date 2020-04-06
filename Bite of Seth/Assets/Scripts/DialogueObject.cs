using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueObject", menuName = "Dialogue Object", order = 0)]
public class DialogueObject : ScriptableObject{

    [Header("First time Dialogue")]
    public List<string> dialogueStrings = new List<string>();

    [Header("Follow on dialogue")]
    public DialogueObject endDialogue;
}