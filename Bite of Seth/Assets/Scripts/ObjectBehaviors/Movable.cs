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
    public GameObject tempCollider;
    private GameObject tc = null;

    void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }
    public void StartMovement(Vector2 desiredMovement, float _speed)
    {
        // return if movement blocked by game manager
        if (ServiceLocator.Get<GameManager>().lockMovement)
        {
            return;
        }

        lookingDirection = desiredMovement.normalized;
        targetPosition = GridNav.WorldToGridPosition(rigidbody.position) + desiredMovement;
        isMoving = true;
        speed = _speed;

        //Put a temporary collider
        if (tc != null)
        {
            Destroy(tc);
        }
        tc = Instantiate(tempCollider, targetPosition, Quaternion.identity) as GameObject;
        tc.transform.parent = gameObject.transform;
        //Debug.Log("Collider temporário");
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            // isMoving == true
            // return if movement blocked by game manager
            if (ServiceLocator.Get<GameManager>().lockMovement)
            {
                return;
            }
            isMoving = !GridNav.MoveToFixed(rigidbody, targetPosition, speed);
            if (isMoving == false)
            {
                gameObject.SendMessage("OnStopedMoving", SendMessageOptions.DontRequireReceiver);
            }
        }
        if (tc != null && isMoving == false)
        {            
            //Remove the additional collider
            Destroy(tc);
        }
    }
}
