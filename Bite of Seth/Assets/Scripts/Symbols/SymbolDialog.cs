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

    bool open = false;

    // Start is called before the first frame update
    void Start() {
        dialogueBox.SetActive(false);
        open = false;
        LoreManager lm = ServiceLocator.Get<LoreManager>();
        lm.dialog = this;
    }

    // Update is called once per frame
    void Update(){
        if(open && Input.GetKeyDown(KeyCode.H)){
            Close();
        }
    }

    public void Present(Lore l) {
        dialogueBox.SetActive(true);
        dialogueTitle.text = l.title;  
        dialogueIcon.sprite = l.icon;     
        inst = StartCoroutine(TypeText(l.text));
        open = true;
    }

    public void Close(){
        StopCoroutine(inst);
        dialogueBox.SetActive(false);
        open = false;
    }

    IEnumerator TypeText(string text) {
        dialogueText.text = "";
        foreach (char c in text.ToCharArray()) {
            yield return new WaitForSeconds(delay);
            dialogueText.text += c;
        }
    }
}
