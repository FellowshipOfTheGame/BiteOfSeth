using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 3f;
    public LayerMask movementCollisionMask;
    private new Rigidbody2D rigidbody;
    private bool isMoving = false;
    private Vector2 targetPosition = Vector2.zero;
    void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (isMoving == false)
        {
            Vector2 desiredMovement = CheckInput();
            bool shouldMove = false;
            if (desiredMovement != Vector2.zero)
            {
                shouldMove = GridNav.GetObjectsInPath(rigidbody, desiredMovement, movementCollisionMask).Count == 0;
            }
            if (shouldMove)
            {
                targetPosition = GridNav.WorldToGridPosition(transform.position) + desiredMovement;
                isMoving = true;
            }
        }
        else
        {
            // isMoving == true
            isMoving = !GridNav.MoveToFixed(rigidbody, targetPosition, movementSpeed);
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

}
