using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehavior : MonoBehaviour
{
    private bool collected = false;
    
    public void Collect()
    {
        gameObject.SetActive(false);
        collected = true;
    }
}
