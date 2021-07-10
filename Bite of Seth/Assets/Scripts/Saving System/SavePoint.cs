using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{

    public BoxCollider2D col;
    public DoorTrigger trigger;
    public AudioObject congratsSfx;
    public AudioObject doorSfx;

    private bool auxLoad = true;

    // Start is called before the first frame update
    void Start()
    {
        // Start with a opened door
        trigger.SetState(true);
        Invoke("DisAuxLoad", 1f);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") {

            // Closes the door
            trigger.SetState(false);

            // Disable this trigger
            col.enabled = false;

            // Save this SavePoint as the current for the Player
            collision.gameObject.GetComponent<PlayerController>().SetSavePoint(this);

            if (!auxLoad) {
                // Player SAVED this SavePoint, not LOADED
   
                // Play SFX:
                if (congratsSfx) {
                    ServiceLocator.Get<AudioManager>().PlayAudio(congratsSfx);
                }
                if (doorSfx) {
                    ServiceLocator.Get<AudioManager>().PlayAudio(doorSfx);
                }

                ServiceLocator.Get<GameManager>().SaveSpawnPoint(gameObject.transform.position);
            }               
        }
    }

    // Auxiliar function to help manipulate when a player loaded os saved the SavePoint
    void DisAuxLoad()
    {
        auxLoad = false;
    }

}
