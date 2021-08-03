using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movable))]
public class PushableBehavior : MonoBehaviour
{
    private Movable movable = null;
    public float normalPushSpeed = 1f;
    public float logicPushSpeed = 4f;
    public float pushSpeed = 1f;
    private bool willNotFall = false;

    [SerializeField]
    private LayerMask collisionMask = default;

    private void Awake()
    {
        movable = gameObject.GetComponent<Movable>();
    }

    public bool Push(Vector2 desiredMovement)
    {
        LogicMovable lm = GetComponent<LogicMovable>();
        if ((lm == null || !lm.enabled) && desiredMovement.normalized == Vector2.up)
        {
            return false;
        }

        FallBehavior fb = GetComponent<FallBehavior>();
        if (fb != null && fb.enabled) {
            willNotFall = !fb.ShouldFall();
        } else {
            willNotFall = true;
        }

        if (!movable.isMoving && GridNav.GetObjectsInPath(movable.rigidbody.position, desiredMovement, collisionMask, gameObject).Count == 0 && willNotFall)
        {
            movable.StartMovement(desiredMovement, pushSpeed);
            if(fb != null) {
                fb.isRolling = true;
                fb.rollingDirection = desiredMovement.x;
            }

            return true;
        }
        return false;
    }

    public void ChangeToLogicSpeed()
    {
        pushSpeed = logicPushSpeed;
    }

    public void ChangeToNormalSpeed()
    {
        pushSpeed = normalPushSpeed;
    }

}
