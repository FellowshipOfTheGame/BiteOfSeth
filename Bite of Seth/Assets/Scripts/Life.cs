using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
