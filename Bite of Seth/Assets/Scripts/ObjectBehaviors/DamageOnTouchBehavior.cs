using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouchBehavior : MonoBehaviour
{
    public float damage = 0;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (damage > 0) 
        {
            if (LayerMask.NameToLayer("Player") == collision.gameObject.layer)
            {
                // kill player
                ServiceLocator.gameManager.KillPlayer();
            }
            else
            {
                // kill other entities here
            }
        }
    }
}
