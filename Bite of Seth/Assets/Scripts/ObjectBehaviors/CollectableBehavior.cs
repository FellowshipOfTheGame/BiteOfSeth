using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBehavior : MonoBehaviour
{
    private bool collected = false;
    public int points = 1;
    public AudioObject collectSFX = null;

    public void Collect()
    {
        if (!collected)
        {
            GameManager gm = ServiceLocator.Get<GameManager>();
            gm.AddScore(points);
            gm.PrintScore();
            gameObject.SetActive(false);
            collected = true;

            ServiceLocator.Get<AudioManager>().PlayAudio(collectSFX);
        }
    }

    public void Decollect()
    {
        if (collected)
        {
            GameManager gm = ServiceLocator.Get<GameManager>();
            gm.AddScore(-points);
            gm.PrintScore();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController p = collision.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            Collect();
        }
    }
}
