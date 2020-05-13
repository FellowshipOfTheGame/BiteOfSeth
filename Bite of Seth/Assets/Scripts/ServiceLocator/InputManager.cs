using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Input Manager")]
public class InputManager : GameService
{
    public KeyCode dialogueKey = KeyCode.None;

    public override void Update()
    {
        CheckInput();
    }
    public void CheckInput()
    {
        if (Input.GetKeyDown(dialogueKey))
        {
            Debug.Log("dialogue key pressed");
        }
    }
}
