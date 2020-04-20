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
            FindObjectOfType<ScoreCounter>().AddPoints(points);
            gameObject.SetActive(false);
            collected = true;
        }
    }

    public void Decollect()
    {
        if (collected)
        {
            FindObjectOfType<ScoreCounter>().AddPoints(-points);
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
