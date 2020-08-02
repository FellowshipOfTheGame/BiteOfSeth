using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoreBook : MonoBehaviour {

    public GameObject entryPrefab;
    public Transform summary;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddEntry(Lore l, SymbolDialog dialog){
        Button b = Instantiate(entryPrefab, summary).GetComponent<Button>();
        b.gameObject.SetActive(true);
        b.onClick.AddListener( delegate{ dialog.Present(l); } );
    }

    public void RemoveEntry(int index){
        Destroy(summary.GetChild(index + 1).gameObject);
    }
}
