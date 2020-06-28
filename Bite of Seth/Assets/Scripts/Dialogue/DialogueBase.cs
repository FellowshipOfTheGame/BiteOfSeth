using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue Object")]
public class DialogueBase : ScriptableObject{
    [System.Serializable]
    public class Info{
        public CharacterInfo character;
        [TextArea(4, 8)]
        public string myText;
    }
    
    [Header("Insert dialogue info below")]
    public int dialogueID;
    public string dialogueTitle;
    public bool isAutoTriggered;
    public Info[] dialogueInfo;
    
    
}
