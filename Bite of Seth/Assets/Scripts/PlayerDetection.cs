using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{

    ChangeToWall wall;

    void Start(){
        wall = gameObject.transform.parent.GetComponent<ChangeToWall>();
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.tag == "Player") {
            wall.animator.SetBool("target", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            wall.animator.SetBool("target", false);
            wall.Change();
            enabled = false;
        }
    }

}
