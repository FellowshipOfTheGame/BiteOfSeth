using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehavior : MonoBehaviour
{
    private bool collected = false;
    public int points;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(LayerMask.LayerToName(collision.collider.gameObject.layer) == "Player")
        {
            collision.collider.gameObject.GetComponent<ScoreCounter>().AddPoints(points);
            gameObject.SetActive(false);
            collected = true;
        }
    }
}
