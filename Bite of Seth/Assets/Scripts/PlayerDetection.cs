using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            gameObject.transform.parent.GetComponent<ChangeToWall>().Change();
            enabled = false;
        }
    }

}
