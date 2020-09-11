using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBehavior : MonoBehaviour
{
    protected bool collected = false;
    public int points = 1;
    public AudioObject collectSFX = null;
    public AutoDestroyBurst particle;

    public virtual void Collect()
    {
        if (!collected)
        {
            //Update level score
            GameManager gm = ServiceLocator.Get<GameManager>();
            gm.AddLevelScore(points);
            gm.PrintLevelScore();
            collected = true;

            //Play sfx
            ServiceLocator.Get<AudioManager>().PlayAudio(collectSFX);
           
            particle.transform.SetParent(null);
            particle.Play();
            gameObject.SetActive(false);
        }
    }

    public virtual void Decollect()
    {
        if (collected)
        {
            //Update level score
            GameManager gm = ServiceLocator.Get<GameManager>();
            gm.AddLevelScore(-points);
            gm.PrintLevelScore();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Player collision results in collect
        PlayerController p = collision.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            Collect();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //Player collision results in collect
        PlayerController p = other.gameObject.GetComponent<PlayerController>();
        if (p != null) {
            Collect();
        }
    }
}
