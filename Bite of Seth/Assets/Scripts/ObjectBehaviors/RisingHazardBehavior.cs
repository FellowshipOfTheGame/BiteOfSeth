using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingHazardBehavior : MonoBehaviour
{

    public Vector2 direction;
    public Vector2 maxScale;
    public Vector2 minScale;
    public Vector2 expandingPerSec;

    private Vector2 dirNorm;
    private Vector2 size;
    private bool isMoving = false;
    private bool expand = true;
    private SpriteRenderer sprite;

    private Vector2 initialSize;
    private Vector2 initialPosition;
    private bool initialExpand;

    private bool activated;

    // Start is called before the first frame update
    void Start()
    {
        //size = transform.localScale;
        sprite = GetComponent<SpriteRenderer>();
        size = sprite.size;
        //Debug.Log(size.x+" "+size.y);
        dirNorm = direction.normalized;
        activated = false;

        initialSize = sprite.size;
        initialPosition = transform.position;
        initialExpand = expand;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isMoving) {

            if (expand) {

                //Expand
                sprite.size += new Vector2(expandingPerSec.x, expandingPerSec.y);

                //Reposition
                transform.position = new Vector3(transform.position.x + (dirNorm.x * expandingPerSec.x / 2),
                                                    transform.position.y + (dirNorm.y * expandingPerSec.y / 2),
                                                    transform.position.z);
                //If it reachs the final position, stop
                if (sprite.size.x >= maxScale.x || sprite.size.y >= maxScale.y) {
                    isMoving = false;
                }

            } else {

                //Shrink
                sprite.size -= new Vector2(expandingPerSec.x, expandingPerSec.y);

                //Reposition
                transform.position = new Vector3(transform.position.x - (dirNorm.x * expandingPerSec.x / 2),
                                                    transform.position.y - (dirNorm.y * expandingPerSec.y / 2),
                                                    transform.position.z);
                //If it reachs the final position, stop
                if (sprite.size.x <= minScale.x || sprite.size.y <= minScale.y) {
                    isMoving = false;
                }

            }
        }
    }

    public void StartExpanding()
    {
        activated = true;
        isMoving = true;
        expand = true;
        Debug.Log("COMEÇOU");
    }

    public void StartShrinking()
    {
        activated = true;
        isMoving = true;
        expand = false;
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(activated && collision.tag == "Player") {
            collision.GetComponent<PlayerController>().Die();
        }
    }

    public void Reset()
    {
        sprite.size = initialSize;
        transform.position = initialPosition;
        expand = initialExpand;
        isMoving = false;
    }

}
