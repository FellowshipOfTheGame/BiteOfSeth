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

    private void Awake()
    {
        movable = gameObject.GetComponent<Movable>();
        gameObject.transform.parent = null;
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
    }

    private void FixedUpdate()
    {
        if (!movable.isMoving)
        {
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
                        }
                    }

                    // object ahead, collectible?
                    CollectibleBehavior collectible = objects[0].GetComponent<CollectibleBehavior>();
                    if (collectible != null) {
                        collectible.Collect();
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
        return desiredMovement;
    }

    public void AssignCheckpoint(CheckpointBehavior c)
    {
        currentCheckpoint = c;
    }
}
