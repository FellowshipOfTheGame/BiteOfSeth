using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Movable))]
public class PlayerController : MonoBehaviour
{
    private Movable movable = null;
    public float movementSpeed = 5f;
    public LayerMask movementCollisionMask;
    public CheckpointBehavior currentCheckpoint = null;
    public LayerMask walkOnLayerMask = default;
    private Animator animator = null;
    private bool pushing = false;
    private bool holding = false;
    private bool dying = false;
    public LayerMask holdableLayerMask = default;
    public float dyingTimer = 3f;

    private void Awake()
    {
        movable = gameObject.GetComponent<Movable>();
        gameObject.transform.parent = null;
    }
    private void Start()
    {
        holdableLayerMask = LayerMask.GetMask("Boulder");
        animator = gameObject.GetComponent<Animator>();
        GameManager gm = ServiceLocator.Get<GameManager>();
        if (gm.player == null)
        {
            gm.player = this;
        }
        else
        {
            Debug.Log("Second instance of player instantiated, destrying it");
            Destroy(this);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UseCheckpoint();
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

        Vector2 animationDiretion = movable.lookingDirection;
        // check if grounded
        if (!pushing && GridNav.GetObjectsInPath(movable.rigidbody.position, 0.6f*GridNav.down, walkOnLayerMask, gameObject).Count == 0)
        {
            animationDiretion = Vector2.up;
        }

        // check if is holding some holdable object
        holding = (GridNav.GetObjectsInPath(movable.rigidbody.position, GridNav.up, holdableLayerMask, gameObject).Count > 0);

        animator.SetFloat("Horizontal", animationDiretion.x);
        animator.SetFloat("Vertical", animationDiretion.y);
        animator.SetBool("Walking", movable.isMoving);        
        animator.SetBool("Pushing", pushing);
        animator.SetBool("Holding", holding);
        animator.SetBool("Dying", dying);
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
                        bool pushed = pushable.Push(desiredMovement);
                        if (pushed)
                        {
                            movable.StartMovement(desiredMovement, pushable.pushSpeed);
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
        if (currentCheckpoint)
        {
            currentCheckpoint.SetCheckpointActive(false);
        }
        currentCheckpoint = c;
    }
    
    public void UseCheckpoint()
    {
        if (currentCheckpoint != null)
        {
            movable.rigidbody.position = currentCheckpoint.transform.position;
            movable.isMoving = false;
            currentCheckpoint.RewindRoom();
            ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().ResetChoices();
        }
    }

    private void Revive()
    {
        Debug.Log("ACABOU DE MORRER");
        movable.enabled = true;
        UseCheckpoint();
        dying = false;
    }

    public void Die()
    {
        movable.enabled = false;
        dying = true;
        Debug.Log("COMECOU A MORRER");
        Invoke("Revive", dyingTimer);
    }

}
