using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBehavior : MonoBehaviour {

    public float propagationTime = 0.5f;

    private void TryToBreakAtDirection(Vector2 direction)
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
        TryToBreakAtDirection(GridNav.up);
        TryToBreakAtDirection(GridNav.up + GridNav.right);
        TryToBreakAtDirection(GridNav.right);
        TryToBreakAtDirection(GridNav.down + GridNav.right);
        TryToBreakAtDirection(GridNav.down);
        TryToBreakAtDirection(GridNav.down + GridNav.left);
        TryToBreakAtDirection(GridNav.left);
        TryToBreakAtDirection(GridNav.up + GridNav.left);
    }

}
