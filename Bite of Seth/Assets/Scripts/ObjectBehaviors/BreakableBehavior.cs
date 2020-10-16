using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBehavior : MonoBehaviour {

    public float propagationTime = 0.5f;

    private void TryBreakAtDirection(Vector2 direction)
    {
        List<GameObject> objects = GridNav.GetObjectsInPath(transform.position, direction, gameObject);
        foreach (GameObject g in objects) {
            BreakableBehavior breakable = g.GetComponent<BreakableBehavior>();
            if (breakable != null) {
                breakable.Break();
            }
        }
    }

    public void Break()
    {
        gameObject.SetActive(false);
        //Propagate the destruction
        Invoke("BreakPropagation", propagationTime);
        Destroy(gameObject, propagationTime + 0.5f);
    }

    private void BreakPropagation()
    {
        TryBreakAtDirection(GridNav.up);
        TryBreakAtDirection(GridNav.down);
        TryBreakAtDirection(GridNav.left);
        TryBreakAtDirection(GridNav.right);
    }

}
