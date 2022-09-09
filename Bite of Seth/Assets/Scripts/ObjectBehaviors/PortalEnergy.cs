using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalEnergy : MonoBehaviour
{
    private bool collected = false;
    public UnityEvent onCollected;

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
        if(collision.tag == "Player" && !collected) {
            collected = true;
            onCollected.Invoke();
            //gameObject.SetActive(false);
        }
    }

}
