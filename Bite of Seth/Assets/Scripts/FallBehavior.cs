using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallBehavior : MonoBehaviour
{
    // THE ROCK 'N' ROLL SCRIPT
    public float speed = 1f;
    public LayerMask fallMask;
    public LayerMask rollMask;
    private new Rigidbody2D rigidbody;
    private bool isMoving = false;
    private Vector2 targetPosition = Vector2.zero;

    void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {        
        if (!isMoving)
        {
            // check if should fall
            if (GridNav.GetObjectsInPath(GridNav.WorldToGridPosition(rigidbody.position), GridNav.down, fallMask, gameObject).Count == 0)
            {
                targetPosition = GridNav.WorldToGridPosition(rigidbody.position) + GridNav.down;
                isMoving = true;
            }
            //check if standing on a round object
            else if (GridNav.GetObjectsInPath(rigidbody.position, GridNav.down, rollMask, gameObject).Count > 0)
            {
                // room to roll left
                if (GridNav.GetObjectsInPath(rigidbody.position + GridNav.left, GridNav.down, fallMask, gameObject).Count == 0)
                {
                    targetPosition = GridNav.WorldToGridPosition(rigidbody.position) + GridNav.down / 2 + GridNav.left;
                    isMoving = true;
                }
                // room to roll right
                else if (GridNav.GetObjectsInPath(rigidbody.position + GridNav.right, GridNav.down, fallMask, gameObject).Count == 0)
                {
                    targetPosition = GridNav.WorldToGridPosition(rigidbody.position) + GridNav.down / 2 + GridNav.right;
                    isMoving = true;
                }            
            }
        }
        if (isMoving)
        {
            isMoving = !GridNav.MoveToFixed(rigidbody, targetPosition, speed);
        }
    }
}

