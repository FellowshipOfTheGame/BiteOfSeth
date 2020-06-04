using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Movable))]
public class PlayerController : MonoBehaviour
{
    private Movable movable = null;
    public float movementSpeed = 3f;
    public LayerMask movementCollisionMask;
    private CheckpointBehavior currentCheckpoint = null;
    private Animator animator = null;
    private bool pushing = false;

    private void Awake()
    {
        movable = gameObject.GetComponent<Movable>();
        gameObject.transform.parent = null;
    }
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentCheckpoint != null)
            {
                currentCheckpoint.RewindRoom();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<GameObject> objects = GridNav.GetObjectsInPath(movable.rigidbody.position, movable.lookingDirection, gameObject);
            foreach (GameObject g in objects)
            {
                BreakableBehavior breakable = g.GetComponent<BreakableBehavior>();
                if (breakable != null)
                {
                    breakable.Break();
                }
            }
        }        
        animator.SetBool("Walking", movable.isMoving);
        animator.SetBool("Pushing", pushing);
        animator.SetFloat("Horizontal", movable.lookingDirection.x);
        animator.SetFloat("Vertical", movable.lookingDirection.y);
    }

    private void FixedUpdate()
    {
        if (!movable.isMoving)
        {
            pushing = false;
            Vector2 desiredMovement = CheckInput();
            if (desiredMovement != Vector2.zero)
            {
                List<GameObject> objects = GridNav.GetObjectsInPath(movable.rigidbody.position, desiredMovement, movementCollisionMask, gameObject);
                if (objects.Count == 0)
                {
                    // nothing on the way, move freely
                    movable.StartMovement(desiredMovement, movementSpeed);
                }
                else if (objects.Count == 1)
                {
                    // object ahead, pushable?
                    PushableBehavior pushable = objects[0].GetComponent<PushableBehavior>();
                    if (pushable != null)
                    {
                        bool pushed = pushable.Push(desiredMovement, movementSpeed);
                        if (pushed)
                        {
                            movable.StartMovement(desiredMovement, movementSpeed);
                            pushing = true;
                        }
                    }

                    // object ahead, collectable?
                    CollectableBehavior collectable = objects[0].GetComponent<CollectableBehavior>();
                    if (collectable != null) {
                        collectable.Collect();
                        movable.StartMovement(desiredMovement, movementSpeed);
                    }
                }
                else
                {
                    Debug.LogWarning(" more than 1 object ahead");
                }  
            }
        }
    }
    
    private Vector2 CheckInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 desiredMovement = Vector2.zero;
        // preference for horizontal movement over vertical 
        if (horizontal != 0)
        {
            if (horizontal > 0)
            {
                desiredMovement = GridNav.right;
            }
            else
            {
                desiredMovement = GridNav.left;
            }
        }
        else if (vertical != 0)
        {
            if (vertical > 0)
            {
                desiredMovement = GridNav.up;
            }
            else
            {
                desiredMovement = GridNav.down;
            }
        }
        if (desiredMovement != Vector2.zero)
        {
            movable.lookingDirection = desiredMovement;
        }        
        return desiredMovement;
    }

    public void AssignCheckpoint(CheckpointBehavior c)
    {
        currentCheckpoint = c;
    }
}
