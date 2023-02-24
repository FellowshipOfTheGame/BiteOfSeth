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

    public List<GameObject> pastStatues;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        // Start with a opened door
        trigger.SetState(true);
        Invoke("DisAuxLoad", 1f);
        gm = ServiceLocator.Get<GameManager>();

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

                // **** Player SAVED this SavePoint ****
   
                // Play SFX:
                if (congratsSfx) {
                    ServiceLocator.Get<AudioManager>().PlayAudio(congratsSfx);
                }
                if (doorSfx) {
                    ServiceLocator.Get<AudioManager>().PlayAudio(doorSfx);
                }

                gm.SaveSpawnPoint(gameObject.transform.position);
            } else {

                // **** Player LOADED this SavePoint ****

                // Load the last puzzle
                PuzzleManager pm = gm.GetLevelPuzzleManager();
                pm.LoadPuzzle(gm.GetLoadedStatuesOrder());
                
                foreach(GameObject statue in pastStatues) {
                    // COUNT ALL PAST STATUES
                    if (statue.GetComponent<DialogueBehavior>().puzzleStatue) {
                        pm.AddTipStatue();
                    }
                    
                }
            }
        }
    }

    // Auxiliar function to help manipulate when a player loaded or saved the SavePoint
    void DisAuxLoad()
    {
        auxLoad = false;
    }

}
