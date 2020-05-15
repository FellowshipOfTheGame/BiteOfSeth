using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movable : MonoBehaviour
{
    public new Rigidbody2D rigidbody;
    private float speed = 0f;
    public bool isMoving = false;
    public Vector2 lookingDirection = Vector2.zero;
    private Vector2 targetPosition = Vector2.zero;

    void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }
    public void StartMovement(Vector2 desiredMovement, float _speed)
    {
        lookingDirection = desiredMovement.normalized;
        targetPosition = GridNav.WorldToGridPosition(rigidbody.position) + desiredMovement;
        isMoving = true;
        speed = _speed;
    }
    private void FixedUpdate()
    {
        if (isMoving)
        {
            // isMoving == true
            isMoving = !GridNav.MoveToFixed(rigidbody, targetPosition, speed);
            if (isMoving == false)
            {
                gameObject.SendMessage("OnStopedMoving", SendMessageOptions.DontRequireReceiver) ;
            }
        }
    }
}
