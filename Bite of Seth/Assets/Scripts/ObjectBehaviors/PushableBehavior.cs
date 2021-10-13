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

    private Vector2 lastDirection = Vector2.zero;
    public bool logicPush = false;

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
            lastDirection = desiredMovement;
            return true;
        }
        return false;
    }

    public void ChangeToLogicSpeed()
    {
        pushSpeed = logicPushSpeed;
        logicPush = true;
    }

    public void ChangeToNormalSpeed()
    {
        pushSpeed = normalPushSpeed;
        logicPush = false;
    }

    // receiver for Movable message
    public void OnStopedMoving()
    {

        if (!logicPush) return;

        List<GameObject> oip = null;
        DamageOnTouchBehavior dotb = GetComponent<DamageOnTouchBehavior>();

        //Get objects in the next fall tile
        oip = GridNav.GetObjectsInPath(GridNav.WorldToGridPosition(movable.rigidbody.position), lastDirection, collisionMask, gameObject);

        if (oip != null && oip.Count > 0) {
            if (dotb != null) {
                dotb.TryToDestroy(oip);
            }
        }

    }

}
