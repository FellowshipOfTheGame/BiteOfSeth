using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Movable))]
public class PlayerController : MonoBehaviour
{
   
    public float normalMovementSpeed = 5f;
    public float logicMovementSpeed = 10f;
    private float movementSpeed = 5f; 
    public LayerMask movementCollisionMask;
    public CheckpointBehavior currentCheckpoint = null;
    public LayerMask walkOnLayerMask = default;
    private Animator animator = null;
    private bool pushing = false;
    private bool holding = false;
    private bool dying = false;
    private bool climbing = false;
    public LayerMask holdableLayerMask = default;
    public float dyingTimer = 3f;
    private LogicMovable lm = null;

    [HideInInspector]
    public Movable movable = null;

    public Transform detectIntPos;

    private CameraFollow cf;
    private AudioManager am;
   
    public AudioObject climbingSfx = null;
    public AudioObject walkingSfx = null;
    private bool playingWalkingSfx = true;

    private void Awake()
    {
        movable = gameObject.GetComponent<Movable>();
        gameObject.transform.parent = null;
        cf = FindObjectOfType<CameraFollow>();
        am = ServiceLocator.Get<AudioManager>();
    }

    private void Start()
    {
        lm = GetComponent<LogicMovable>();
        movementSpeed = normalMovementSpeed;
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
        movable.SetMovableSfx(walkingSfx);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !dying && ServiceLocator.Get<GameManager>().lockMovement == 0)
        {
            UseCheckpoint();
        }

        if (Input.GetKeyDown(KeyCode.Space) && !dying && ServiceLocator.Get<GameManager>().lockMovement == 0)
        {
            List<GameObject> objects = GridNav.GetObjectsInPath(movable.rigidbody.position, movable.lookingDirection, gameObject);
            foreach (GameObject g in objects)
            {
                ExplodeBehavior eb = g.GetComponent<ExplodeBehavior>();
                if (eb != null)
                {
                    eb.StartTimerToExplode();
                }
            }
        }

        Vector2 animationDiretion = movable.lookingDirection;
        // check if its not grounded
        climbing = !pushing && GridNav.GetObjectsInPath(movable.rigidbody.position, 0.6f * GridNav.down, walkOnLayerMask, gameObject).Count == 0;

        if (climbing && playingWalkingSfx) {
            movable.SetMovableSfx(climbingSfx);
            playingWalkingSfx = false;
        }
        else if (!climbing && !playingWalkingSfx) {
            movable.SetMovableSfx(walkingSfx);
            playingWalkingSfx = true;
        }

        // check if is holding some holdable object
        holding = (GridNav.GetObjectsInPath(movable.rigidbody.position, GridNav.up, holdableLayerMask, gameObject).Count > 0);

        animator.SetFloat("Horizontal", animationDiretion.x);
        animator.SetFloat("Vertical", animationDiretion.y);
        animator.SetBool("Walking", movable.isMoving);        
        animator.SetBool("Pushing", pushing);
        animator.SetBool("Holding", holding);
        animator.SetBool("Dying", dying);
        animator.SetBool("Climbing", climbing);

        detectIntPos.position = (Vector2)transform.position + movable.lookingDirection;

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

        Vector2 desiredMovement = Vector2.zero;
        if (ServiceLocator.Get<GameManager>().lockMovement > 0) {
            desiredMovement = Vector2.zero;
            movable.lookingDirection = desiredMovement;
            return desiredMovement;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
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
        if (c.IsActive()) {
            return;
        }
        if (currentCheckpoint)
        {
            currentCheckpoint.SetCheckpointActive(false);
        }
        currentCheckpoint = c;
    }
    
    public void UseCheckpoint()
    {
        movable.StopSfx();
        if (currentCheckpoint != null)
        {
            //Debug.Log("CHECKPOINT");
            PortalBehavior pb = currentCheckpoint.GetComponent<PortalBehavior>();
            if(pb != null) {
                pb.ChangeObjectBehavior(gameObject.GetComponent<TransportableBehavior>());
            }
            cf.enabled = false;
            movable.rigidbody.position = currentCheckpoint.transform.position;
            gameObject.transform.position = currentCheckpoint.transform.position;
            cf.SmoothTransition();
            cf.enabled = true;
            movable.isMoving = false;
            movable.DestroyTempCol();
            currentCheckpoint.RewindRoom();
            ServiceLocator.Get<GameManager>().GetLevelPuzzleManager().ResetChoices();


            movable.enabled = false;
            Invoke("EnableMovement", 1f);

        }
    }

    private void Revive()
    {
        //Debug.Log("ACABOU DE MORRER");
        
        UseCheckpoint();
        dying = false;
        //Invoke("EnableMovement", 1f);
        movable.enabled = true;
    }

    public void EnableMovement()
    {
        movable.enabled = true;
    }

    public void Die()
    {
        movable.StopSfx();
        movable.enabled = false;
        dying = true;
        //Debug.Log("COMECOU A MORRER");
        Invoke("Revive", dyingTimer);
    }

    public bool IsDying()
    {
        return dying;
    }

    public void ChangeToLogicSpeed()
    {
        movementSpeed = logicMovementSpeed;
    }

    public void ChangeToNormalSpeed()
    {
        movementSpeed = normalMovementSpeed;
    }

}
