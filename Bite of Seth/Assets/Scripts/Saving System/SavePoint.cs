using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{

    public BoxCollider2D col;
    public DoorTrigger trigger;
    public AudioObject congratsSfx;
    public AudioObject doorSfx;

    // Start is called before the first frame update
    void Start()
    {
        // Start with a opened door
        trigger.SetState(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") {
            // Closes the door and make it stay closed
            if (congratsSfx) {
                ServiceLocator.Get<AudioManager>().PlayAudio(congratsSfx);
            }
            if (doorSfx) {
                ServiceLocator.Get<AudioManager>().PlayAudio(doorSfx);
            }
            trigger.SetState(false);
            col.enabled = false;
            collision.gameObject.GetComponent<PlayerController>().SetSavePoint(this);
        }
    }

}
