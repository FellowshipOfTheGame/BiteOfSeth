using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallMovement : MonoBehaviour
{

    public float gravity = 10f;
    //private float movementSpeed;

    private new Rigidbody2D rigidbody;

    [Tooltip("Reference a grid to be used to movement snaping, or leave it as null and set the grid size and offset manualy")]
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Vector2 gridSize = Vector2.one;
    [SerializeField]
    private Vector2 gridOffset = Vector2.zero;
    private Vector2 targetPosition = Vector2.zero;
    private bool isMoving = false;
    private bool sideMove = false;

    void Awake()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();

        if (grid != null) {
            gridSize = grid.cellSize;
            gridOffset = grid.transform.position + grid.cellSize / 2;
        } else {
            if (gridSize.x == 0 || gridSize.y == 0) {
                Debug.LogError("FallMovement gridSize x or y should not be zero ");
            }
        }
    }

    void Start()
    {
        rigidbody.MovePosition(RoundPosition(transform.position));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving == false) {
            Vector2 desiredMovement;
            //PRIORITY ORDER: DOWN, DOWN LEFT AND DOWN RIGTH;

            //Try to move one tile down
            if (ValidateMovement(desiredMovement = new Vector2(0f, -gridSize.y))) {
                targetPosition = RoundPosition(transform.position) + desiredMovement;
                isMoving = true;
                sideMove = false;
            }
            //If it failed, verify if can move left and left/down
             else if (!sideMove && ValidateMovement((desiredMovement = new Vector2(-gridSize.x, 0f)))
                    && ValidateMovement((desiredMovement = new Vector2(-gridSize.x, -gridSize.y)))) {
                targetPosition = RoundPosition(transform.position) + desiredMovement;
                isMoving = true;
                sideMove = true;
            }
            //If it failed, verify if can move right and right/down
            else if (!sideMove && ValidateMovement((desiredMovement = new Vector2(gridSize.x, 0f))) 
                    && ValidateMovement((desiredMovement = new Vector2(gridSize.x, -gridSize.y)))) {
                targetPosition = RoundPosition(transform.position) + desiredMovement;
                isMoving = true;
                sideMove = true;
            }
        } else {
            // isMoving == true
            MoveTo(targetPosition);
        }
    }

    private bool ValidateMovement(Vector2 desiredMovement)
    {
        //raycast to see if there are colliders on the way of movement
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, desiredMovement, desiredMovement.magnitude);
        //Debug.DrawRay(transform.position, desiredMovement, Color.blue);
        //hit array has at least, automaticaly, one element that is the collider of this rigidbody.
        //Then, if hit has more than one element, it means that the desiredMovement is not valid.
        //Otherwise, if hit has just one element, then the desiredMovement is valid:
        return (hit.Length == 1);
    }

    private Vector2 RoundPosition(Vector2 position)
    {
        // round the position to snap to the center of a grid cell
        position.x = gridSize.x * Mathf.Floor(position.x / gridSize.x) + gridOffset.x;
        position.y = gridSize.y * Mathf.Floor(position.y / gridSize.y) + gridOffset.y;
        return position;
    }

    private void MoveTo(Vector2 targetPosition)
    {
        Vector3 increment = new Vector3(targetPosition.x, targetPosition.y, 0) - transform.position;
        if (increment.magnitude > 0.02f) // max diference between position and target position to consider
        {
            increment = increment.normalized * Mathf.Pow(Time.fixedDeltaTime, 2) * gravity / 2; /// S = (g * t^2) / 2
            rigidbody.MovePosition(transform.position + increment);
        } else {
            isMoving = false;
            rigidbody.MovePosition(targetPosition); // final move so the rigidbody ends exactly on the target point
        }

    }

}
