using System.Collections;
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

    public bool cameraFocus = true;
    public float focusTime = 1.5f;

    private SavePointTrigger savePointTrigger;

    private void Start()
    {
        startPos = doorMovable.rigidbody.position;
        savePointTrigger = GetComponent<SavePointTrigger>();
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
            if (sfx) {
                ServiceLocator.Get<AudioManager>().PlayAudio(sfx);
            }
            if (cameraFocus) {
                ServiceLocator.Get<GameManager>().FocusCameraOnXDuringYSeconds(gameObject.transform.position, focusTime);
            }
            if(savePointTrigger != null) {
                savePointTrigger.TriggerActiveTrue();
            }
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
            if (sfx) {
                ServiceLocator.Get<AudioManager>().PlayAudio(sfx);
            }
            if (savePointTrigger != null) {
                savePointTrigger.TriggerActiveFalse();
            }
        }
        
    }
}
