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
        lookingDirection = desiredMovement.normalized;
        targetPosition = GridNav.WorldToGridPosition(rigidbody.position) + desiredMovement;
        isMoving = true;
        speed = _speed;

        //Put a temporary collider
        tc = Instantiate(tempCollider, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        if (tc) 
        {
            tc.transform.parent = gameObject.transform;
            //set the temporary collider position(in Grid)
            tc.GetComponent<TemporaryCollider>().SetPosition(targetPosition);
        }

    }
    private void FixedUpdate()
    {
        if (isMoving)
        {
            // isMoving == true
            isMoving = !GridNav.MoveToFixed(rigidbody, targetPosition, speed);
            if (isMoving == false)
            {
                //Remove the additional collider
                if (tc) 
                {
                    Destroy(tc);
                }
                gameObject.SendMessage("OnStopedMoving", SendMessageOptions.DontRequireReceiver) ;
            }
        }
    }
}
