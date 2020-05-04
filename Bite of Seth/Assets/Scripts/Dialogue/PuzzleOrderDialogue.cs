using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOrderDialogue : MonoBehaviour
{

    TextScript ts;
    public int id;
    private bool selected = false;

    // Start is called before the first frame update
    void Start()
    {
        ts = GetComponentInChildren<TextScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!selected) {
            if(selected = ts.TryToDialogue())
            {
                PuzzleManager.instance.SelectStatue(id);
            }
        }
    }

}
