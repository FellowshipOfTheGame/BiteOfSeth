using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBehavior : MonoBehaviour
{
    TextScript ts;

    // Start is called before the first frame update
    void Start()
    {
        ts = GetComponentInChildren<TextScript>();
    }

    // Update is called once per frame
    void Update()
    {
        ts.TryToDialogue();
    }
}
