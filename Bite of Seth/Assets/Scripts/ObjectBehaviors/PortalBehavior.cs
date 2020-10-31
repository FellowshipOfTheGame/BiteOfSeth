using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehavior : MonoBehaviour
{

    public GameObject otherSidePortalRef;
    private bool playerInRange;
    private GameObject playerRef;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && playerInRange) {
            playerRef.transform.position = otherSidePortalRef.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player")) {
            playerRef = collision.gameObject;
            playerInRange = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("Player")) {
            playerRef = null;
            playerInRange = false;
        }
    }



}
