using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Lore", menuName = "Lore Object")]
public class Lore : ScriptableObject {
    
    public string title;

    [TextArea(15, 20)]
    public string text;
    public Sprite icon, shadow, symbol;
}
