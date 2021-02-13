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

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        pc = gameObject.GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        movable = gameObject.GetComponent<Movable>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (pc) {
            animator.SetBool("Logic", this.enabled);
        }*/
    }

    public void EnableLogicMovement()
    {
        if (pc) {
            animator.SetBool("Logic", true);
        }
        this.enabled = true;
    }

    public void DisableLogicMovement()
    {
        if (pc) {
            animator.SetBool("Logic", false);
            pc.movable.DestroyTempCol();
        }
        this.enabled = false;
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
