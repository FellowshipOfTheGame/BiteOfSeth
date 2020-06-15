using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public List<DoorTrigger> triggers = new List<DoorTrigger>();
    private bool isClosed = true;
    public AudioObject sfx = null;

    void FixedUpdate()
    {
        bool openDoor = triggers.Count > 0;
        foreach (DoorTrigger t in triggers)
        {
            // check to see if every pressure plate is pressed
            if (t.GetState() == false)
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
