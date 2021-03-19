using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject _player;
    private PlayerController pc;
    private GameObject player
    {
        get
        {
            if (_player == null)
            {
                _player = FindObjectOfType<PlayerController>().gameObject;
            }
            return _player;
        }
    }
    public Vector3 offset;

    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
    private Vector3 playerPos;

    public float maxDistToInstantTransition = 1f;

    void LateUpdate()
    {
        playerPos = player.transform.position + offset;
        if (Vector3.Distance(transform.position,playerPos) <= maxDistToInstantTransition) {
            transform.position = playerPos;
        } else {
            transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref velocity, smoothTime);
        }
        //transform.position = playerPos;
        //transform.position = Vector3.Lerp(transform.position, playerPos, smoothTime);
    }

}
