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
    private GameObject[] tc = new GameObject[2];
    private GameManager gameManager = null; // cache do manager

    void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        gameManager = ServiceLocator.Get<GameManager>();
    }

    public void StartMovement(Vector2 desiredMovement, float _speed)
    {
        // return if movement blocked by game manager
        if (ServiceLocator.Get<GameManager>().lockMovement > 0)
        {
            return;
        }
        lookingDirection = desiredMovement.normalized;
        
        targetPosition = GridNav.WorldToGridPosition(rigidbody.position) + desiredMovement;

        isMoving = true;
        speed = _speed;
        
        //Put a temporary collider
        if (tc[0] != null)
        {
            Destroy(tc[0]);
        }
        if (tc[1] != null) {
            Destroy(tc[1]);
        }

        if (desiredMovement.y > GridNav.down.y && desiredMovement.y < 0) {
            Vector2 fix = new Vector2(0f, desiredMovement.y);
            tc[0] = Instantiate(tempCollider, targetPosition - fix, Quaternion.identity) as GameObject;
            tc[1] = Instantiate(tempCollider, targetPosition - fix + GridNav.down, Quaternion.identity) as GameObject;
            tc[0].transform.parent = gameObject.transform;
            tc[1].transform.parent = gameObject.transform;
        } else {
            tc[0] = Instantiate(tempCollider, targetPosition, Quaternion.identity) as GameObject;
            tc[0].transform.parent = gameObject.transform;
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            // isMoving == true
            // return if movement blocked by game manager
            if (gameManager.lockMovement > 0)
            {
                return;
            }

            isMoving = !GridNav.MoveToFixed(rigidbody, targetPosition, speed);

            if (isMoving == false)
            {
                if (tc[0] != null) {
                    //Remove the additional collider
                    Destroy(tc[0]);
                }
                if (tc[1] != null) {
                    Destroy(tc[1]);
                }
                gameObject.SendMessage("OnStopedMoving", SendMessageOptions.DontRequireReceiver);
            }
        }     
    }
}
