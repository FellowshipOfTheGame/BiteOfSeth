using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public LayerMask pressMask = default;
    public bool isPressed = false;

    private void FixedUpdate()
    {
        isPressed = GridNav.GetObjectsInPath(gameObject.transform.position, GridNav.up, pressMask, gameObject).Count > 0;
    }
}
