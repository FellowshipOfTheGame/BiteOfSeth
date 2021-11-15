using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEnergy : MonoBehaviour
{
    private bool collected = false;

    // Start is called before the first frame update
    void Start()
    {
        collected = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsCollected()
    {
        return collected;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") {
            collected = true;
            gameObject.SetActive(false);
        }
    }

}
