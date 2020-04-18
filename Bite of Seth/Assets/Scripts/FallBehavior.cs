using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FallBehavior : MonoBehaviour
{
    // THE ROCK 'N' ROLL SCRIPT
    private Movable movable = null;
    public LayerMask fallMask;
    public LayerMask rollMask;
    public float fallSpeed = 3f;
    public bool canKillPlayer;
    private List<GameObject> objectsInPath;
    private GameObject player;

    void Awake()
    {
        movable = gameObject.GetComponent<Movable>();
    }

    private void FixedUpdate()
    {        
        if (!movable.isMoving)
        {

            // check if should fall
            if ((objectsInPath = GridNav.GetObjectsInPath(GridNav.WorldToGridPosition(movable.rigidbody.position), GridNav.down, fallMask, gameObject)).Count == 0)
            {
                movable.StartMovement(GridNav.down, fallSpeed);
            }
            //check if standing on a round object
            else if (GridNav.GetObjectsInPath(movable.rigidbody.position, GridNav.down, rollMask, gameObject).Count > 0)
            {
                // room to roll left
                if (GridNav.GetObjectsInPath(movable.rigidbody.position + GridNav.left, GridNav.down, fallMask, gameObject).Count == 0)
                {
                    movable.StartMovement(GridNav.down / 2 + GridNav.left, fallSpeed);
                }
                // room to roll right
                else if (GridNav.GetObjectsInPath(movable.rigidbody.position + GridNav.right, GridNav.down, fallMask, gameObject).Count == 0)
                {
                    movable.StartMovement(GridNav.down / 2 + GridNav.right, fallSpeed);
                }            
            }
            //check if can kill the player
            else if (canKillPlayer) 
            {
                foreach(GameObject go in objectsInPath) 
                {
                    //check if is standing on the player
                    if (LayerMask.LayerToName(go.layer) == "Player")
                    {
                        go.GetComponent<Life>().Kill();
                    }
                }
            }
        }
    }
}

