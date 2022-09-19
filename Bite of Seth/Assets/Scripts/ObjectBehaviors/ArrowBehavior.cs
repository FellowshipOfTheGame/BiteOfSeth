using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehavior : MonoBehaviour
{
    public float speed;
    public bool isMoving = false;
    public GameManager gameManager;
    public new Rigidbody2D rigidbody;
    public Vector2 lookingDirection = Vector2.zero;
    private Vector2 targetPosition = Vector2.zero;

    private void Awake()
    {
        gameManager = ServiceLocator.Get<GameManager>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMovement(Vector2 desiredMovement, float _speed)
    {
        // return if movement blocked by game manager
        if (ServiceLocator.Get<GameManager>().lockMovement > 0) {
            gameObject.SetActive(false);
            Destroy(gameObject);
            return;
        }

        lookingDirection = desiredMovement.normalized;

        targetPosition = GridNav.WorldToGridPosition(rigidbody.position) + desiredMovement;

        isMoving = true;
        speed = _speed;
    }

    private void FixedUpdate()
    {
        if (isMoving) {
            // isMoving == true
            // return if movement blocked by game manager
            if (gameManager.lockMovement > 0) {
                return;
            }

            isMoving = !GridNav.MoveToFixed(rigidbody, targetPosition, speed);

            if (!isMoving) {
                ContinueMovement();
            }
        }
    }

    public void ContinueMovement()
    {
        StartMovement(lookingDirection, speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            collision.GetComponent<PlayerController>().Die();
        } else{
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }


}
