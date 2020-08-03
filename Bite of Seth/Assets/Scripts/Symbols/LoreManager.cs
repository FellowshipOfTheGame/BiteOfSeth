using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/LoreManager")]
public class LoreManager : GameService {

    public List<Lore> lore;
    public int total = 0;

    public GameObject dialogPrefab, bookPrefab;

    SymbolDialog dialog;
    LoreBook book;

    public override void Start() {
        book = Instantiate(bookPrefab).GetComponent<LoreBook>();
        dialog = Instantiate(dialogPrefab).GetComponent<SymbolDialog>();
        
    }

    public void Learn(Lore l){
        lore.Add(l);
        book.AddEntry(l, dialog);
        dialog.Present(l);
    }

    public void Forget(Lore l){
        if(lore.Contains(l)){
            book.RemoveEntry(lore.IndexOf(l));
            lore.Remove(l);
        }
    }
}
