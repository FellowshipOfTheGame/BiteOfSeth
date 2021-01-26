using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplodeBehavior : MonoBehaviour
{
    public float delayTime = 3f;
    public TextMesh timerText;
    public Transform tt;
    private float timeCounter = 0f;
    public Animator anim;

    private void TryToDestroyAtDirection(Vector2 direction)
    {
        List<GameObject> objects = GridNav.GetObjectsInPath(transform.position, direction, gameObject);

        foreach (GameObject g in objects) {
            
            //Try to break breakable bases
            BreakableBehavior breakable = g.GetComponent<BreakableBehavior>();
            if (breakable != null) {
                breakable.Break();
            }

            ExplodeBehavior ep = g.GetComponent<ExplodeBehavior>();
            if (ep != null) {
                ep.Explode();
            }

            //Try to kill player
            if (g.layer == LayerMask.NameToLayer("Player")) {
                // kill player
                ServiceLocator.Get<GameManager>().KillPlayer();
            } else {
                // kill other entities here
            }

        }
    }

    public void StartTimerToExplode()
    {
        //Propagate the destruction
        timeCounter = delayTime;
        Invoke("Explode", delayTime);
        anim.SetBool("Explosion", true);
    }

    public void Explode()
    {
        gameObject.SetActive(false);
        TryToDestroyAtDirection(GridNav.up);
        TryToDestroyAtDirection(GridNav.up + GridNav.right);
        TryToDestroyAtDirection(GridNav.right);
        TryToDestroyAtDirection(GridNav.down + GridNav.right);
        TryToDestroyAtDirection(GridNav.down);
        TryToDestroyAtDirection(GridNav.down + GridNav.left);
        TryToDestroyAtDirection(GridNav.left);
        TryToDestroyAtDirection(GridNav.up + GridNav.left);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        tt.eulerAngles = Vector3.zero;
        if(timeCounter > 0) {
            timeCounter -= Time.fixedDeltaTime;
            if (timerText != null) {
                timerText.text = "" + (int)timeCounter;
            }
        }
    }

}
