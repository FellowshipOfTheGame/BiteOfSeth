using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallBehavior : MonoBehaviour
{
    public float fallSpeed = 1f;
    public LayerMask fallCollisionMask;
    public bool roll = false;
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
            if (GridNav.GetObjectsInPath(rigidbody, GridNav.down, fallCollisionMask).Count == 0)
            {
                targetPosition = GridNav.WorldToGridPosition(rigidbody.position) + GridNav.down;
                isMoving = true;
            }
        }
        if (isMoving)
        {
            isMoving = !GridNav.MoveToFixed(rigidbody, targetPosition, fallSpeed);
        }
    }
}

