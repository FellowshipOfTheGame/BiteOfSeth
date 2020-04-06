using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectItem : MonoBehaviour
{
    public GameObject item = null;
    public Vector2 facingDirection = Vector2.zero;
    public bool itemDetected = false;
    public bool isMoving = false;
    private Vector2 lastPositionScanned = Vector2.zero;
    private Vector2 nextPositionScan = Vector2.zero;

    void Awake(){
        facingDirection = gameObject.GetComponent<Movement>().facingDirection;
        isMoving = gameObject.GetComponent<Movement>().isMoving;
    }

    void Update()
    {
        facingDirection = gameObject.GetComponent<Movement>().facingDirection;
        isMoving = gameObject.GetComponent<Movement>().isMoving;

        nextPositionScan.x = transform.position.x + facingDirection.x;
        nextPositionScan.y = transform.position.y + facingDirection.y;

        if(nextPositionScan != lastPositionScanned && isMoving == false){
            itemDetected = checkForStatue(facingDirection);
        }

    }

    private bool checkForStatue(Vector2 facingDirection){
        //throws a raycast in the direction the character is facing acording to the last movement or attempt of movement
        //looking for a statue with help of its tag. If the statue is found storages object reference for interaction phase and returns true. 
        lastPositionScanned.x = transform.position.x + facingDirection.x;
        lastPositionScanned.y = transform.position.y + facingDirection.y;

        RaycastHit2D sensor = Physics2D.Raycast(transform.position, facingDirection, facingDirection.magnitude);

        if(sensor.collider != null && sensor.transform.tag == "Statue"){
            Debug.Log("Found statue.");
            item = sensor.collider.gameObject;

            return true;
        }

        item = null;
        return false;
    }



}
