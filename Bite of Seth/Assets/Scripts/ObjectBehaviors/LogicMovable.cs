using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicMovable : MonoBehaviour
{

    private Movable movable = null;
    public LayerMask movementCollisionMask;
    public AudioObject fallSound = null;
    private Animator animator = null;
    private PlayerController pc = null;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        movable = gameObject.GetComponent<Movable>();
        pc = gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pc) {
            animator.SetBool("Logic", this.enabled);
        }
    }

    private void OnStopedMoving()
    {
        if (enabled) {
            List<GameObject> oip = null;
            //Get objects in the next fall tile
            oip = GridNav.GetObjectsInPath(GridNav.WorldToGridPosition(movable.rigidbody.position), movable.lookingDirection, movementCollisionMask, gameObject);
            if (oip.Count > 0) {
                if (fallSound != null) {
                    ServiceLocator.Get<AudioManager>().PlayAudio(fallSound);
                }
            } else {
                //Continue moving with same direction and same speed
                movable.ContinueMovement();
            }
        }
    }

    public bool IsMoving()
    {
        return movable.isMoving;
    }

}
