using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearableBehavior : MonoBehaviour
{
    public LayerMask disappearMask = default;
    public ParticleSystem particle;
    public Animator animator;
    //Disappears when the player exits this gameObject
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // check if collision layer is in disappearMask
        if (disappearMask == (disappearMask | (1 << collision.collider.gameObject.layer))) {
            //Destroy(gameObject);
            this.GetComponent<BoxCollider2D>().enabled = false;
            animator.SetTrigger("break");
        }
    }

    public void Explode(){
        particle.Play();
    }
}
