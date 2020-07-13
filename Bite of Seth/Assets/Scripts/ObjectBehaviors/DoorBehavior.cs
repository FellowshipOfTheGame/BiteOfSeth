﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public List<DoorTrigger> triggers = new List<DoorTrigger>();
    private bool isClosed = true;
    public AudioObject sfx = null;
    public Movable doorMovable;
    private Vector3 startPos;
    public Vector3 openDistance;
    public float openSpeed;

    private void Start()
    {
        startPos = doorMovable.rigidbody.position;
    }

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
            Vector3 pos = GridNav.WorldToGridPosition(doorMovable.rigidbody.position);
            doorMovable.StartMovement(startPos + openDistance - pos, openSpeed);
            GetComponent<Collider2D>().enabled = false;
            ServiceLocator.Get<AudioManager>().PlayAudio(sfx);
        }
    }

    void CloseDoor()
    {
        if (!isClosed)
        {
            isClosed = true;
            Vector3 pos = GridNav.WorldToGridPosition(doorMovable.rigidbody.position);
            doorMovable.StartMovement(startPos - pos, openSpeed);
            GetComponent<Collider2D>().enabled = true;
            ServiceLocator.Get<AudioManager>().PlayAudio(sfx);
        }
        
    }
}
