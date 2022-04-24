using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointTrigger : MonoBehaviour
{
    public GameObject savePoint;

    public void TriggerActiveTrue()
    {
        savePoint.SetActive(true);
    }

    public void TriggerActiveFalse()
    {
        savePoint.SetActive(false);
    }

}
