using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    private new Rigidbody2D rigidbody; 
    public float movementSpeed = 3f;
    [Tooltip("Reference a grid to be used to movement snaping, or leave it as null and set the grid size and offset manualy")]
    [SerializeField]
    private Grid grid = null;
    [SerializeField]
    private Vector2 gridSize = Vector2.one;
    [SerializeField]
    private Vector2 gridOffset = Vector2.zero;    
    private Vector2 targetPosition = Vector2.zero;
    public bool isMoving = false;
    public Vector2 facingDirection = Vector2.zero;

    void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();

        if (grid != null)
        {
            gridSize = grid.cellSize;
            gridOffset = grid.transform.position + grid.cellSize/2;
        }
        else
        {
            if (gridSize.x == 0|| gridSize.y == 0)
            {
                Debug.LogError("PlayerMovement gridSize x or y should not be zero ");
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving == false)
        {
            Vector2 desiredMovement = checkInput();
            bool shouldMove = false;

            if (desiredMovement != Vector2.zero)
            {
                shouldMove = validateMovement(desiredMovement);
            }           
            if (shouldMove)
            {
                targetPosition = roundPosition(transform.position) + desiredMovement;
                isMoving = true;
            }

            
        }
        else
        {
            // isMoving == true
            moveTo(targetPosition);
        }        
    }

    private Vector2 checkInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 desiredMovement = Vector2.zero;
        // preference for horizontal movement over vertical 
        if (horizontal != 0)
        {
            if (horizontal > 0)
            {
                desiredMovement.x = gridSize.x;
            }
            else
            {
                desiredMovement.x = -gridSize.x;
            }
            facingDirection = desiredMovement;
        }
        else if (vertical != 0)
        {
            if (vertical > 0)
            {
                desiredMovement.y = gridSize.y;
            }
            else
            {
                desiredMovement.y = -gridSize.y;
            }
            facingDirection = desiredMovement;
        }
        
        return desiredMovement;
    }
    
    private bool validateMovement(Vector2 desiredMovement)
    {
        //raycast to see if there is a collider on the way of movement
        RaycastHit2D hit = Physics2D.Raycast(transform.position, desiredMovement, desiredMovement.magnitude);
        if (hit.collider != null)
        {
            return false;
        }
        return true;
    }

    private Vector2 roundPosition(Vector2 position)
    {
        // round the position to snap to the center of a grid cell
        position.x = gridSize.x * Mathf.Floor(position.x / gridSize.x) + gridOffset.x;
        position.y = gridSize.y * Mathf.Floor(position.y / gridSize.y) + gridOffset.y;
        return position;
    }

    private void moveTo(Vector2 targetPosition)
    {
        Vector3 increment = new Vector3(targetPosition.x, targetPosition.y, 0) - transform.position;
        if (increment.magnitude > 0.02f) // max diference between position and target position to consider
        {
            increment = increment.normalized * Time.fixedDeltaTime * movementSpeed;
            rigidbody.MovePosition(transform.position + increment);
        }
        else
        {
            isMoving = false;
            rigidbody.MovePosition(targetPosition); // final move so the rigidbody ends exactly on the target point
        }
        
    }

}
