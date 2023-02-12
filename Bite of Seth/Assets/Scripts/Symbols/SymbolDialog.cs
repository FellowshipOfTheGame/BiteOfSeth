using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolDialog : MonoBehaviour {

    public Lore info;
    private Image icon;
    private bool unlocked;

    public void SetUnlocked(bool value) {
        unlocked = value;
        if (icon == null) icon = this.GetComponent<Image>();
        if (unlocked) icon.sprite = info.icon;
        else icon.sprite = info.shadow;
    }

    public bool GetUnlocked() {
        return unlocked;
    }
}
