using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : DoorTrigger
{
    public GameObject pressedSprite;
    public GameObject unpressedSprite;
    public LayerMask pressMask = default;
    public bool isPressed = false;
    public AudioObject sfx = null;

    private void FixedUpdate()
    {
        bool pressedBefore = isTriggered;
        SetState(GridNav.GetObjectsInPath(gameObject.transform.position, GridNav.up, pressMask, gameObject).Count > 0);
        if (pressedBefore != isTriggered)
        {
            if(pressedSprite) pressedSprite.SetActive(isTriggered);
            if(unpressedSprite) unpressedSprite.SetActive(!isTriggered);
            ServiceLocator.Get<AudioManager>().PlayAudio(sfx);
        }
    }
}
