using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public List<PressurePlate> plates = new List<PressurePlate>();

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
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void CloseDoor()
    {
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
