using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapBehavior : MonoBehaviour
{

    public float activatedTimer = 3f;
    public float deactivatedTimer = 3f;
    public float delayTimer = 0f;

    private float timeCounter = 0f;
    private bool activated = false;
    private bool firstTime = true;
    
    private BoxCollider2D bc;
    public SpriteRenderer sr;
    private AudioSource sfx;
    private bool blocked = false;
    private bool firstPlay = false;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        bc.enabled = false;
        activated = false;
        firstTime = true;
        sfx = GetComponent<AudioSource>();
        //sr.color = Color.gray;
        //sr.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeCounter += Time.fixedDeltaTime;

        //Control the trap activation and deactivation timers 
        if (!firstTime) {
            if (!activated && timeCounter >= deactivatedTimer) {
                ActivateTrap();
            } else if (activated && timeCounter >= activatedTimer) {
                DeactivateTrap();
            }
        } else {
            if (!activated && timeCounter >= (delayTimer)) {
                ActivateTrap();
                firstTime = false;
            } else if (activated && timeCounter >= (delayTimer)) {
                DeactivateTrap();
                firstTime = false;
            }
        } 
        
    }

    private void ActivateTrap()
    {
        blocked = false;
        activated = true;
        bc.enabled = true;
        //sr.enabled = true;
        anim.SetBool("Activated", true);
        ResetCounter();
        Invoke("PlaySfx", 0.1f);
        
    }

    private void DeactivateTrap()
    {
        activated = false;
        bc.enabled = false;
        //sr.enabled = false;
        anim.SetBool("Activated", false);
        ResetCounter();
        if (firstPlay && sfx) {
            sfx.Play();
        }
    }

    private void ResetCounter()
    {
        timeCounter = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
            ServiceLocator.Get<GameManager>().KillPlayer();
        } else {
            //Se for qualquer outro objeto que se move(pedra, por exemplo)
            //então desativa o colisor
            bc.enabled = false;
            //sr.enabled = false;
            anim.SetBool("Activated", false);
            blocked = true;
            //Debug.Log("BLOQUEADO por " + collision.collider.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        blocked = false;
        //Debug.Log("DESBLOQUEADO por " + collision.collider.gameObject);
    }

    private void PlaySfx()
    {
        if (!blocked) {
            if (sfx) {
                sfx.Play();
            }
            firstPlay = true;
        } else {
            firstPlay = false;
        }
    }

}
