﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearableBehavior : MonoBehaviour
{

    //Disappears when the player exits this gameObject
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (LayerMask.LayerToName(collision.gameObject.layer) == "Player") {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }


}
