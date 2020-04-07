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

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
