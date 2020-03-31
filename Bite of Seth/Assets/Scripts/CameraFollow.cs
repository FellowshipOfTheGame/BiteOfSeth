using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
        offset.x = 0; 
        offset.y = 0;
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
