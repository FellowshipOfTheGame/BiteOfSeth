using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningPopupController : MonoBehaviour
{
    public GameObject warningPopup = null;
    public GameObject finalStatue;
    private PuzzleFinalDialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = finalStatue.GetComponent<PuzzleFinalDialogue>();
    }

    // Update is called once per frame
    void Update()
    {
        if (warningPopup.activeSelf) {
            if (Input.GetKeyDown(KeyCode.E)) {
                Close();
            }
        }
    }

    public void Close()
    {
        warningPopup.SetActive(false);

        ServiceLocator.Get<GameManager>().ResumePlayerControls();
        //Debug.Log($"Close TriggerWarningAlert lock-- ({gm.lockMovement})");
        dialogue.canDialogue = true;
    }

}
