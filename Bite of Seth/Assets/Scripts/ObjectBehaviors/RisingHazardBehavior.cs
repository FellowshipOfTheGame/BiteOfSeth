using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingHazardBehavior : MonoBehaviour
{

    public Vector2 direction;
    public Vector2 maxScale;
    public Vector2 expandingPerSec;

    private Vector2 dirNorm;
    private Vector2 size;
    private bool isMoving = false;
    
    // Start is called before the first frame update
    void Start()
    {
        size = transform.localScale;
        dirNorm = direction.normalized;
        StartExpanding();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isMoving) {
            //Expand
            transform.localScale = new Vector3( transform.localScale.x + expandingPerSec.x,
                                                transform.localScale.y + expandingPerSec.y,
                                                transform.localScale.z);
            //Reposition
            transform.position = new Vector3(   transform.position.x + (dirNorm.x * expandingPerSec.x / 2),
                                                transform.position.y + (dirNorm.y * expandingPerSec.y / 2),
                                                transform.position.z);

            //If it reachs the final position, stop
            if(transform.localScale.x >= maxScale.x || transform.localScale.y >= maxScale.y) {
                isMoving = false;
            }
        }
    }

    public void StartExpanding()
    {
        isMoving = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") {
            collision.GetComponent<PlayerController>().Die();
        }
    }

}
