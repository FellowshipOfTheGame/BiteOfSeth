using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapBehavior : MonoBehaviour
{

    public float activatedTimer = 3f;
    public float deactivatedTimer = 3f;

    private float timeCounter = 0f;
    private bool activated = false;
    
    private BoxCollider2D bc;
    public SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        bc.enabled = false;
        sr.color = Color.gray;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeCounter += Time.fixedDeltaTime;

        //Control the trap activation and deactivation timers 
        if (!activated && timeCounter >= deactivatedTimer) {
            ActivateTrap();
        } else if(activated && timeCounter >= activatedTimer) {
            DeactivateTrap();
        }
        
    }

    private void ActivateTrap()
    {
        activated = true;
        bc.enabled = true;
        sr.color = Color.red;
        ResetCounter();
    }

    private void DeactivateTrap()
    {
        activated = false;
        bc.enabled = false;
        sr.color = Color.gray;
        ResetCounter();
    }

    private void ResetCounter()
    {
        timeCounter = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
            ServiceLocator.Get<GameManager>().KillPlayer();
        } else {
            //Se for qualquer outro objeto que se move(pedra, por exemplo)
            //então desativa o colisor
            bc.enabled = false;
            sr.color = Color.gray;
        }
    }

}
