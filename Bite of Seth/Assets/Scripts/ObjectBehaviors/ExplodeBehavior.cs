using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeBehavior : MonoBehaviour
{
    public float delayTime = 1f;

    private void TryToDestroyAtDirection(Vector2 direction)
    {
        List<GameObject> objects = GridNav.GetObjectsInPath(transform.position, direction, gameObject);

        foreach (GameObject g in objects) {
            
            //Try to break breakable bases
            BreakableBehavior breakable = g.GetComponent<BreakableBehavior>();
            if (breakable != null) {
                breakable.Break();
            }

            //Try to kill player
            if (LayerMask.NameToLayer("Player") == g.layer) {
                // kill player
                ServiceLocator.Get<GameManager>().KillPlayer();
            } else {
                // kill other entities here
            }

        }
    }

    public void Explode()
    {
        //Propagate the destruction
        Invoke("DestroyPropagating", delayTime);
    }

    private void DestroyPropagating()
    {
        gameObject.SetActive(false);
        TryToDestroyAtDirection(GridNav.up);
        TryToDestroyAtDirection(GridNav.down);
        TryToDestroyAtDirection(GridNav.left);
        TryToDestroyAtDirection(GridNav.right);
        Destroy(gameObject);
    }

}
