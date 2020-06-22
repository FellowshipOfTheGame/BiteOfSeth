using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallBehavior : MonoBehaviour
{
    // THE ROCK 'N' ROLL SCRIPT
    private Movable movable = null;
    public bool canKill = false;
    public LayerMask startFallMask;
    public LayerMask fallingMask;
    public LayerMask rollMask;
    public float fallSpeed = 3f;
    public AudioObject fallSound = null;
    private RollDelay rd;
    public Animator animator = null;
    private bool isRolling = false;
    private bool startedFalling = false;

    //IMPORTANT: the difference between startFallMask and fallingMask is that the second one doesnt include the Player layer.

    void Start()
    {
        movable = gameObject.GetComponent<Movable>();
        rd = gameObject.GetComponent<RollDelay>();
        startedFalling = false;
    }

    private void Update()
    {
        if (animator != null)
        {
            animator.SetBool("rolling", isRolling);
            // TODO: flip sprite to roll left/right
        }
    }

    private void FixedUpdate()
    {

        if (!movable.isMoving)
        {
            isRolling = false;
            // check if should fall
            if ((!startedFalling && GridNav.GetObjectsInPath(GridNav.WorldToGridPosition(movable.rigidbody.position), GridNav.down, startFallMask, gameObject).Count == 0)
               || (startedFalling && GridNav.GetObjectsInPath(GridNav.WorldToGridPosition(movable.rigidbody.position), GridNav.down, fallingMask, gameObject).Count == 0))
            {
                movable.StartMovement(GridNav.down, fallSpeed);
            }
            //check if standing on a round object
            else if (GridNav.GetObjectsInPath(movable.rigidbody.position, GridNav.down, rollMask, gameObject).Count > 0)
            {
                // room to roll left
                if (GridNav.GetObjectsInPath(movable.rigidbody.position, GridNav.left, startFallMask, gameObject).Count == 0
                    && GridNav.GetObjectsInPath(movable.rigidbody.position + GridNav.left, GridNav.down, startFallMask, gameObject).Count == 0)
                {
                    if (rd)
                    {
                        //Roll delay
                        if (rd.IsOff()) {
                            rd.TurnOn();
                        } else if (rd.IsFinished()) {
                            movable.StartMovement(GridNav.down / 2 + GridNav.left, fallSpeed);
                            isRolling = true;
                        }
                    }
                    else {
                        movable.StartMovement(GridNav.down / 2 + GridNav.left, fallSpeed);
                        isRolling = true;
                    }
                }
                // room to roll right
                else if (GridNav.GetObjectsInPath(movable.rigidbody.position, GridNav.right, startFallMask, gameObject).Count == 0
                    && GridNav.GetObjectsInPath(movable.rigidbody.position + GridNav.right, GridNav.down, startFallMask, gameObject).Count == 0)
                {
                    if (rd)
                    {
                        //Roll delay
                        if (rd.IsOff()) {
                            rd.TurnOn();
                        } else if (rd.IsFinished()) {
                            movable.StartMovement(GridNav.down / 2 + GridNav.right, fallSpeed);
                            isRolling = true;
                        }
                    }
                    else {
                        movable.StartMovement(GridNav.down / 2 + GridNav.right, fallSpeed);
                        isRolling = true;
                    }
                } 
                else if (rd) 
                {
                    //Then, even after the delay, it couldnt roll, so it will have another delay;
                    if (rd.IsOn()) rd.Restart();
                }
            }
        }

    }
    
    // receiver for Movable message
    private void OnStopedMoving()
    {
        if(rd) rd.TurnOff();

        List<GameObject> oip = null;

        //It didnt start falling yet
        if (!startedFalling){
            //Start falling when, after passing the firts Grid tile, there is nothing to stop the start of the falling in the next fall tile
            if (GridNav.GetObjectsInPath(GridNav.WorldToGridPosition(movable.rigidbody.position), GridNav.down, startFallMask, gameObject).Count == 0) {
                startedFalling = true;
            }
        } else {
            //Its falling
            //Get objects in the next fall tile
            oip = GridNav.GetObjectsInPath(GridNav.WorldToGridPosition(movable.rigidbody.position), GridNav.down, fallingMask, gameObject);
            //If there is no player temporary collider in the objects, then stop falling;
            if (oip.Count > 0 && !ContainsPlayerTempCol(oip)) {
                startedFalling = false;
            }
        }

        if (fallSound != null && movable.lookingDirection == Vector2.down) {
            if (oip == null) {
                //Get objects in the next fall tile
                oip = GridNav.GetObjectsInPath(GridNav.WorldToGridPosition(movable.rigidbody.position), GridNav.down, fallingMask, gameObject);
            }
            if (oip.Count > 0) {
                ServiceLocator.Get<AudioManager>().PlayAudio(fallSound);
            }
        }
    }

    private bool ContainsPlayerTempCol(List<GameObject> oip)
    {
        foreach(GameObject go in oip) {
            if(LayerMask.LayerToName(go.layer) == "TemporaryCollider" && go.transform.parent.tag == "Player") {
                return true;
            }
        }
        return false;
    }

}