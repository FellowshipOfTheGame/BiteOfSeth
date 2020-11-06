using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehavior : MonoBehaviour
{

    public GameObject otherSidePortalRef;
    public bool logicSide = false;
    private bool playerInRange;
    private GameObject playerRef;
    private bool boulderInRange;
    private GameObject boulderRef;
    public bool canTransportBoulder = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && playerInRange && !(playerRef.GetComponent<Movable>().isMoving)) {
            TransportPlayerToPortal(otherSidePortalRef, playerRef, Vector2.zero); 
        }
        if(boulderInRange && !(boulderRef.GetComponent<Movable>().isMoving) && canTransportBoulder) {
            TransportBoulderToPortal(otherSidePortalRef, boulderRef, Vector2.right);
        }
    }

    private void TransportPlayerToPortal(GameObject portal, GameObject obj, Vector2 direction)
    {
        obj.GetComponent<LogicMovable>().enabled = false;
        Movable mov = obj.GetComponent<Movable>();
        mov.rigidbody.position = (Vector3)GridNav.WorldToGridPosition((Vector2)portal.transform.position + direction);
        mov.StoppedMoving();
        mov.isMoving = false;
        if (portal.GetComponent<PortalBehavior>().logicSide) {
            obj.GetComponent<LogicMovable>().enabled = true;
            obj.GetComponent<PlayerController>().ChangeToLogicSpeed();
        } else {
            obj.GetComponent<LogicMovable>().enabled = false;
            obj.GetComponent<PlayerController>().ChangeToNormalSpeed();
        }
    }

    private void TransportBoulderToPortal(GameObject portal, GameObject obj, Vector2 direction)
    {
        obj.GetComponent<LogicMovable>().enabled = false;
        Movable mov = obj.GetComponent<Movable>();
        mov.rigidbody.position = (Vector3)GridNav.WorldToGridPosition((Vector2)portal.transform.position + direction);
        mov.StoppedMoving();
        mov.isMoving = false;
        if (portal.GetComponent<PortalBehavior>().logicSide) {
            obj.GetComponent<LogicMovable>().enabled = true;
            obj.GetComponent<FallBehavior>().enabled = false;
            obj.GetComponent<RollDelay>().enabled = false;
            obj.GetComponent<PushableBehavior>().ChangeToLogicSpeed();
        } else {
            obj.GetComponent<LogicMovable>().enabled = false;
            obj.GetComponent<FallBehavior>().enabled = true;
            obj.GetComponent<RollDelay>().enabled = true;
            obj.GetComponent<PushableBehavior>().ChangeToNormalSpeed();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            playerInRange = true;
            playerRef = collision.gameObject;
        } else if (collision.CompareTag("Boulder")) {
            boulderInRange = true;
            boulderRef = collision.gameObject;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            playerInRange = false;
            playerRef = null;
        }
        else if (collision.CompareTag("Boulder")) {
            boulderInRange = false;
        }

    }



}
