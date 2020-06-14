using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallBehavior : MonoBehaviour
{
    // THE ROCK 'N' ROLL SCRIPT
    private Movable movable = null;
    public bool canKill = false;
    public LayerMask fallMask;
    public LayerMask rollMask;
    public float fallSpeed = 3f;
    public AudioObject fallSound = null;
    private RollDelay rd;
    public Animator animator = null;
    private bool isRolling = false;
    void Start()
    {
        movable = gameObject.GetComponent<Movable>();
        rd = gameObject.GetComponent<RollDelay>();
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
            if (GridNav.GetObjectsInPath(GridNav.WorldToGridPosition(movable.rigidbody.position), GridNav.down, fallMask, gameObject).Count == 0)
            {
                movable.StartMovement(GridNav.down, fallSpeed);
            }
            //check if standing on a round object
            else if (GridNav.GetObjectsInPath(movable.rigidbody.position, GridNav.down, rollMask, gameObject).Count > 0)
            {
                // room to roll left
                if (GridNav.GetObjectsInPath(movable.rigidbody.position, GridNav.left, fallMask, gameObject).Count == 0
                    && GridNav.GetObjectsInPath(movable.rigidbody.position + GridNav.left, GridNav.down, fallMask, gameObject).Count == 0)
                {
                    if (rd)
                    {
                        //Roll delay
                        if (rd.IsOff()) rd.TurnOn();
                        else if (rd.IsFinished()) movable.StartMovement(GridNav.left, fallSpeed);
                    }
                    else {
                        movable.StartMovement(GridNav.left, fallSpeed); 
                        //movable.StartMovement(GridNav.down / 2 + GridNav.left, fallSpeed);
                        isRolling = true;
                    }
                }
                // room to roll right
                else if (GridNav.GetObjectsInPath(movable.rigidbody.position, GridNav.right, fallMask, gameObject).Count == 0
                    && GridNav.GetObjectsInPath(movable.rigidbody.position + GridNav.right, GridNav.down, fallMask, gameObject).Count == 0)
                {
                    if (rd)
                    {
                        //Roll delay
                        if (rd.IsOff()) rd.TurnOn();
                        else if (rd.IsFinished()) movable.StartMovement(GridNav.right, fallSpeed);
                    }
                    else {
                        movable.StartMovement(GridNav.right, fallSpeed); 
                        //movable.StartMovement(GridNav.down / 2 + GridNav.right, fallSpeed);
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
        if (fallSound != null) {
            if (movable.lookingDirection == Vector2.down &&
                GridNav.GetObjectsInPath(GridNav.WorldToGridPosition(movable.rigidbody.position), GridNav.down, fallMask, gameObject).Count > 0)
            {
                ServiceLocator.Get<AudioManager>().PlayAudio(fallSound);
            }
        }
    }

}