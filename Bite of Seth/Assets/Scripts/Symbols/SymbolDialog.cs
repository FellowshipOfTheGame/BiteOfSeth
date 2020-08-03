using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymbolDialog : MonoBehaviour {

    Coroutine inst;
    public GameObject dialogueBox;
    public Text dialogueText, dialogueTitle;
    public Image dialogueIcon;
    public float delay;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Present(Lore l) {
        dialogueBox.SetActive(true);
        dialogueTitle.text = l.title;  
        dialogueIcon.sprite = l.icon;     
        inst = StartCoroutine(TypeText(l.text));
    }

    public void Close(){
        StopCoroutine(inst);
        dialogueBox.SetActive(false);
    }

    IEnumerator TypeText(string text) {
        dialogueText.text = "";
        foreach (char c in text.ToCharArray()) {
            yield return new WaitForSeconds(delay);
            dialogueText.text += c;
        }
    }
}
