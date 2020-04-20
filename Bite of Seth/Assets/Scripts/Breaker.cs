using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Breaker : MonoBehaviour
{
    public new Rigidbody2D rigidbody;
    [SerializeField]
    private LayerMask breakMask = default;
    private List<GameObject> target;
    private BreakableBehavior bh;

    void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Break(Vector2 direction)
    {
        //Check if there is a breakable object in the direction
        if((target = GridNav.GetObjectsInPath(rigidbody.position, direction, breakMask, gameObject)).Count > 0) {
            if ((bh = target[0].GetComponent<BreakableBehavior>()) != null) {
                //Breaks
                bh.Break();
            }
        }
    }
}
