using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    protected bool isTriggered = false;

    public void SetState(bool s)
    {
        isTriggered = s;
    }
    public bool GetState()
    {
        return isTriggered;
    }
}
