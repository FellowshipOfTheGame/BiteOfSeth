﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBehavior : MonoBehaviour
{
    protected bool collected = false;
    public int points = 1;
    public AudioObject collectSFX = null;

    public virtual void Collect()
    {
        if (!collected)
        {
            /*GameManager gm = ServiceLocator.Get<GameManager>();
            gm.AddScore(points);
            gm.PrintScore();*/
            GameManager gm = ServiceLocator.Get<GameManager>();
            gm.AddLevelScore(points);
            gm.PrintLevelScore();
            gameObject.SetActive(false);
            collected = true;

            ServiceLocator.Get<AudioManager>().PlayAudio(collectSFX);
        }
    }

    public virtual void Decollect()
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

    private void OnTriggerEnter2D(Collider2D other) {
        PlayerController p = other.gameObject.GetComponent<PlayerController>();
        if (p != null) {
            Collect();
        }
    }
}
