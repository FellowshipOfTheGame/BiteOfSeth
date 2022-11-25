using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolDialog : MonoBehaviour {

    public Lore info;
    private Image icon;

    // Start is called before the first frame update
    void Start() {
        icon = this.GetComponent<Image>();
        icon.sprite = info.icon;
    }
}
