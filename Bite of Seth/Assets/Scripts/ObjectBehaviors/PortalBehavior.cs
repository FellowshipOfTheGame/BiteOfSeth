using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehavior : MonoBehaviour
{

    public GameObject otherSidePortalRef;
    public bool logicSide = false;
    private bool playerInRange;
    private GameObject playerRef;
    private GameObject boulderRef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && playerInRange && !playerRef.GetComponent<Movable>().isMoving) {
            TransportToPortal(otherSidePortalRef, playerRef, Vector2.zero); 
        }
    }

    private void TransportToPortal(GameObject portal, GameObject obj, Vector2 direction)
    {
        obj.transform.position = (Vector3) GridNav.WorldToGridPosition((Vector2)portal.transform.position + direction);
        if (portal.GetComponent<PortalBehavior>().logicSide) {
            obj.GetComponent<LogicMovable>().enabled = true;
            obj.GetComponent<FallBehavior>().enabled = false;
            obj.GetComponent<RollDelay>().enabled = false;
        } else {
            obj.GetComponent<LogicMovable>().enabled = false;
            obj.GetComponent<FallBehavior>().enabled = true;
            obj.GetComponent<RollDelay>().enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player")) {
            playerRef = collision.gameObject;
            playerInRange = true;
        } else if (collision.CompareTag("Boulder")) {
            boulderRef = collision.gameObject;
            TransportToPortal(otherSidePortalRef, boulderRef, Vector2.right);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Player")) {
            playerRef = null;
            playerInRange = false;
        }
        /*else if (collision.CompareTag("Boulder")) {

        }*/

    }



}
