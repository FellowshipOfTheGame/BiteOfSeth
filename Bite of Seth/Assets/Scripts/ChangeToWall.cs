using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeToWall : MonoBehaviour
{

    public GameObject defaultSprite;
    public GameObject wallSprite;

    public void Change()
    {
        gameObject.layer = 9; //change layer to Wall layer
        defaultSprite.SetActive(false);
        wallSprite.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = true;
    }


}
