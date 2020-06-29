using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonsNavigation : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Button>().Select();
    }
}
