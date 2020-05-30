using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public List<PressurePlate> plates = new List<PressurePlate>();
    private bool isClosed = true;
    public AudioObject sfx = null;

    void FixedUpdate()
    {
        bool openDoor = plates.Count > 0;
        foreach (PressurePlate p in plates)
        {
            // check to see if every pressure plate is pressed
            if (p.isPressed == false)
            {
                openDoor = false;
            }
        }
        if (openDoor)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    void OpenDoor()
    {
        if (isClosed)
        {
            isClosed = false;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            ServiceLocator.Get<AudioManager>().PlayAudio(sfx);
        }
    }

    void CloseDoor()
    {
        if (!isClosed)
        {
            isClosed = true;
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }
        
    }
}
