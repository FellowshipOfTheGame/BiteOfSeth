using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouchBehavior : MonoBehaviour {
    public float damage = 0;

    public void TryToKill(List<GameObject> objects)
    {
        if (damage > 0) {
            foreach (GameObject obj in objects) {
                if (LayerMask.NameToLayer("Player") == obj.layer) {
                    // kill player
                    FallBehavior fb = GetComponent<FallBehavior>();
                    if(fb != null) {
                        fb.enabled = false;
                    }
                    ServiceLocator.Get<GameManager>().KillPlayer();
                } else {
                    // kill other entities here
                }
            }
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (damage > 0) {
            if (LayerMask.NameToLayer("Player") == collision.gameObject.layer) {
                // kill player
                ServiceLocator.Get<GameManager>().KillPlayer();
            } else {
                // kill other entities here
            }
        }
    }*/

}
