using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapBehavior : MonoBehaviour
{

    public float activatedTimer = 3f;
    public float deactivatedTimer = 3f;

    private float timeCounter = 0f;
    private bool activated = false;

    private DamageOnTouchBehavior dot;
    private BoxCollider2D bc;
    public SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        bc.enabled = false;
        dot = GetComponent<DamageOnTouchBehavior>();
        dot.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timeCounter += Time.fixedDeltaTime;

        if (activated) {
            Vector2 dir = new Vector2(0.01f, 0.01f);
            List<GameObject> objects = GridNav.GetObjectsInPath(transform.position, dir, gameObject);
            foreach (GameObject g in objects) {
                //Try to kill player
                if (g.layer == LayerMask.NameToLayer("Player")) {
                    // kill player
                    ServiceLocator.Get<GameManager>().KillPlayer();
                } else {
                    // kill other entities here
                }
            }
        }

        if (!activated && timeCounter >= deactivatedTimer) {
            activated = true;
            bc.enabled = true;
            timeCounter = 0;
            dot.enabled = true;
            sr.color = Color.red;
        } else if(activated && timeCounter >= activatedTimer) {
            activated = false;
            bc.enabled = false;
            timeCounter = 0;
            dot.enabled = false;
            sr.color = Color.white;
        }
        
    }

}
