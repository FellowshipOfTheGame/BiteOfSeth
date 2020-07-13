using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movable))]
public class PushableBehavior : MonoBehaviour
{
    private Movable movable = null;
    [SerializeField]
    private LayerMask collisionMask = default;

    private void Awake()
    {
        movable = gameObject.GetComponent<Movable>();
    }

    public bool Push(Vector2 desiredMovement, float pushSpeed)
    {
        if (desiredMovement.normalized == Vector2.up)
        {
            return false;
        }
        if (!movable.isMoving && GridNav.GetObjectsInPath(movable.rigidbody.position, desiredMovement, collisionMask, gameObject).Count == 0)
        {
            movable.StartMovement(desiredMovement, pushSpeed);
            return true;
        }
        return false;
    }
}
