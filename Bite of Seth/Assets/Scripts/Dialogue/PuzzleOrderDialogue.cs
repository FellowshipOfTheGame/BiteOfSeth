using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleOrderDialogue : MonoBehaviour
{

    TextScript ts;
    private bool selected = false;

    public PuzzleManager.Id id;

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

    public void ResetSelection()
    {
        selected = false;
    }

}
