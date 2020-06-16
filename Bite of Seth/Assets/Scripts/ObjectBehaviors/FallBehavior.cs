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
                if (!startedFalling) {
                    startedFalling = true;
                }
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
                            if (!startedFalling) {
                                startedFalling = true;
                            }
                            isRolling = true;
                        }
                    }
                    else {
                        movable.StartMovement(GridNav.down / 2 + GridNav.left, fallSpeed);
                        if (!startedFalling) {
                            startedFalling = true;
                        }
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
                            if (!startedFalling) {
                                startedFalling = true;
                            }
                            isRolling = true;
                        }
                    }
                    else {
                        movable.StartMovement(GridNav.down / 2 + GridNav.right, fallSpeed);
                        if (!startedFalling) {
                            startedFalling = true;
                        }
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
        List<GameObject> oip;
        if(rd) rd.TurnOff();
        if (fallSound != null) {
            if (movable.lookingDirection == Vector2.down &&
            (oip = GridNav.GetObjectsInPath(GridNav.WorldToGridPosition(movable.rigidbody.position), GridNav.down, fallingMask, gameObject)).Count > 0)
            //GridNav.GetObjectsInPath(GridNav.WorldToGridPosition(movable.rigidbody.position), GridNav.down, startFallMask, gameObject).Count > 0)
            {
                ServiceLocator.Get<AudioManager>().PlayAudio(fallSound);
                //If its not a player temporary collider, then stop falling;
                if (!ContainsPlayerTempCol(oip)) {
                    startedFalling = false;
                }
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