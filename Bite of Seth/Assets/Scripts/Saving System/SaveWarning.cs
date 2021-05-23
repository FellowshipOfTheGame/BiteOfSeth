using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveWarning : MonoBehaviour
{

    public GameObject Text;
    public float secondsToShowSavingText = 3f;

    public void ShowSavingWarning()
    {
        Text.SetActive(true);
        Invoke("HideSavingText", secondsToShowSavingText);
    }

    private void HideSavingText()
    {
        Text.SetActive(false);
    }


}
