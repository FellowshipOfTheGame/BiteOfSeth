using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBehavior : MonoBehaviour
{
    private bool collected = false;
    public int points = 1;

    public void Collect()
    {
        if (!collected)
        {
            ServiceLocator.gameManager.AddScore(points);
            ServiceLocator.gameManager.PrintScore();
            gameObject.SetActive(false);
            collected = true;
        }
    }

    public void Decollect()
    {
        if (collected)
        {
            ServiceLocator.gameManager.AddScore(-points);
            ServiceLocator.gameManager.PrintScore();
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
