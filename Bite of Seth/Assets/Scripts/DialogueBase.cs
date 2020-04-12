using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue Object")]
public class DialogueBase : ScriptableObject{
    [System.Serializable]
    public class Info{
        public string characterName;
        public Sprite portrait;
        [TextArea(4, 8)]
        public string myText;
    }
    
    [Header("Insert dialogue info below")]
    public Info[] dialogueInfo;
}
