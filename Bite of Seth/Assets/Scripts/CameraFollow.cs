using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject _player;
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

    private bool inTransition = false;

    void LateUpdate()
    {
        playerPos = player.transform.position + offset;
        if (!inTransition) {
            transform.position = playerPos;
        } else {
            transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref velocity, smoothTime);
            inTransition = (Vector3.Distance(transform.position, playerPos) > 0.1);
            //transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref velocity, smoothTime);
            //transform.position = Vector3.Lerp(transform.position, playerPos, smoothTime);
        }
    }

    public void SmoothTransition()
    {
        inTransition = true;
    }

}
