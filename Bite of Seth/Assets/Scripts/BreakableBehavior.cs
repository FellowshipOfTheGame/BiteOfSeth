using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBehavior : MonoBehaviour
{
    public float propagationTimeInterval = 0.5f;
    private Breaker br;

    private void Awake()
    {
        br = GetComponent<Breaker>();
    }

    public void Break()
    {
        gameObject.SetActive(false);
        //Propagate the destruction
        Invoke("BreakPropagation", propagationTimeInterval);
        Destroy(gameObject, propagationTimeInterval+0.5f);
    }

    private void BreakPropagation()
    {
        br.Break(GridNav.up);
        br.Break(GridNav.down);
        br.Break(GridNav.left);
        br.Break(GridNav.right);
    }

}
