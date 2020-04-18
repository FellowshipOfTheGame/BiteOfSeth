using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
