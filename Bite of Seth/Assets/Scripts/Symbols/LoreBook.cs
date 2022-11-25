using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoreBook : MonoBehaviour {

    public GameObject entryPrefab;

    private bool loreOpened;
    private List<Button> lorePieces;

    Coroutine inst;
    public GameObject summary;
    public GameObject loreBox;
    public GameObject loreCatalog;
    public Text dialogueText, dialogueTitle;
    public Image dialogueIcon;
    public float delay;

    // Start is called before the first frame update
    void Start() {
        LoreManager lm = ServiceLocator.Get<LoreManager>();
        lm.book = this;

        lorePieces = new List<Button>();
        
        for (int i = 0; i < loreCatalog.transform.childCount; i++) {
            Button piece = loreCatalog.transform.GetChild(i).GetComponent<Button>();
            if (piece != null) lorePieces.Add(piece);
        }

        loreBox.SetActive(false);
        loreOpened = false;
        UpdateContent(lm.collectedLore);
    }

    public void PresentLore(SymbolDialog symbol) {
        if (loreOpened) return;

        summary.SetActive(false);
        loreBox.SetActive(true);
        dialogueTitle.text = symbol.info.title;
        dialogueIcon.sprite = symbol.info.icon;
        inst = StartCoroutine(TypeText(symbol.info.text));
        loreOpened = true;
    }

    public void CloseLore() {
        StopCoroutine(inst);
        summary.SetActive(true);
        loreBox.SetActive(false);
        loreOpened = false;
    }

    IEnumerator TypeText(string text) {
        dialogueText.text = "";
        foreach (char c in text.ToCharArray()) {
            yield return new WaitForSeconds(delay);
            dialogueText.text += c;
        }
    }

    public void AddEntry(Lore l, SymbolDialog dialog){
        
    }

    public void RemoveEntry(int index){
       
    }

    public void UpdateContent(bool[,] collectedLore) {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 6; j++) {
                lorePieces[i * 6 + j].interactable = collectedLore[i, j];
            }
        }
    }
}
