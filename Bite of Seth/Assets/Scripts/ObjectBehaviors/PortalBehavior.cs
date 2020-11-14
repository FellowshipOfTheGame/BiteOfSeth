using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehavior : MonoBehaviour
{

    public GameObject otherSidePortalRef;
    public bool logicSide = false;
    
    //otp: object to transport
    private bool otpInRange;
    private GameObject otpRef;
    private TransportableBehavior otpTranspBeh;

    private bool playerInRange;
    private GameObject playerRef;
    private bool boulderInRange;
    private GameObject boulderRef;

    //public bool canTransportBoulder = false;
    public Vector2 automaticSpawnDir = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(otpInRange && otpTranspBeh.CanTransport(otherSidePortalRef)) {
            otpTranspBeh.TransportToPortal(otherSidePortalRef);
        }

        /*if (Input.GetKeyDown(KeyCode.T) && playerInRange && !(playerRef.GetComponent<Movable>().isMoving)) {
            TransportPlayerToPortal(otherSidePortalRef, playerRef, Vector2.zero); 
        }
        if(boulderInRange &&  && ) {
            TransportBoulderToPortal(otherSidePortalRef, boulderRef, boulderSpawnDir);
        }*/
    }

    public void ChangeObjectBehavior(TransportableBehavior tb)
    {
        tb.ChangeBehavior(GetComponent<PortalBehavior>());
    }

    /*private void TransportPlayerToPortal(GameObject portal, GameObject obj, Vector2 direction)
    {
        obj.GetComponent<LogicMovable>().enabled = false;
        Movable mov = obj.GetComponent<Movable>();
        mov.rigidbody.position = (Vector3)GridNav.WorldToGridPosition((Vector2)portal.transform.position + direction);
        mov.StoppedMoving();
        mov.isMoving = false;
        ChangePlayerToLogic(portal.GetComponent<PortalBehavior>(), obj);
    }

    public void ChangePlayerToLogic(PortalBehavior portal, GameObject obj)
    {
        if (portal.logicSide) {
            Debug.Log("LOGICO");
            obj.GetComponent<LogicMovable>().enabled = true;
            obj.GetComponent<PlayerController>().ChangeToLogicSpeed();
        } else {
            Debug.Log("NORMAL");
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
        ChangeBoulderToLogic(portal.GetComponent<PortalBehavior>(), obj);
        canTransportBoulder = false;
    }

    public void ChangeBoulderToLogic(PortalBehavior portal, GameObject obj)
    {
        if (portal.logicSide) {
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
    }*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        TransportableBehavior tb = other.gameObject.GetComponent<TransportableBehavior>();
        if (tb != null) {
            otpInRange = true;
            otpRef = other.gameObject;
            otpTranspBeh = tb;
        }
        /*if (other.CompareTag("Player")) {
            playerInRange = true;
            playerRef = other.gameObject;
        } else if (other.CompareTag("Boulder")) {
            boulderInRange = true;
            boulderRef = other.gameObject;
        }*/
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        TransportableBehavior tb = other.gameObject.GetComponent<TransportableBehavior>();
        if(otpRef == other.gameObject) {
            otpInRange = false;
            otpRef = null;
            otpTranspBeh = null;
        }
        
        /*if (other.CompareTag("Player")) {
            playerInRange = false;
            playerRef = null;
        }
        else if (other.CompareTag("Boulder")) {
            boulderInRange = false;
        }*/

    }



}
