using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/LoreManager")]
public class LoreManager : GameService {

    public bool[,] collectedLore = new bool[3,6];


    public List<Lore> lore;
    public int total = 0;

    public GameObject dialogPrefab, bookPrefab;

    public SymbolDialog dialog;
    public LoreBook book;

    public void Learn(Lore l){
        lore.Add(l);
        book.AddEntry(l, dialog);
    }

    public void LoadCollectedLore(bool[,] collectedLore) {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 6; j++) {
                this.collectedLore[i, j] = collectedLore[i, j];
            }
        }
        if (book != null) book.UpdateContent(collectedLore);
    }

    public void SetCollectedLore(SymbolBehavior symbol, bool collected) {
        collectedLore[symbol.level, symbol.index] = collected;
        DialogueManager.instance.toggleLoreAlert(true);
        if (book != null) book.UpdateContent(collectedLore);
    }

    public void LearnPastLore(Lore l)
    {
        lore.Add(l);
        book.AddEntry(l, dialog);
    }

    public void Forget(Lore l){
        if(lore.Contains(l)){
            book.RemoveEntry(lore.IndexOf(l));
            lore.Remove(l);
        }
    }
}
