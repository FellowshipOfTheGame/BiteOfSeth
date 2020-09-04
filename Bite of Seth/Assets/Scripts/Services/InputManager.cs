using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Input Manager")]
public class InputManager : GameService
{
    public KeyCode dialogueKey = KeyCode.None;
    public KeyCode audioTestKey = KeyCode.None;
    public AudioObject testAudio = null;
    public override void Update()
    {
        CheckInput();
    }
    public void CheckInput()
    {
        if (Input.GetKeyDown(dialogueKey))
        {
            //Debug.Log("Dialogue key pressed");
        }
        if (Input.GetKeyDown(audioTestKey))
        {
            //ServiceLocator.Get<AudioManager>().PlayAudio(testAudio);
        }
    }
}
