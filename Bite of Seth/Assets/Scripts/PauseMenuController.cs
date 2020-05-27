using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public KeyCode openMenuKey = KeyCode.None;
    public GameObject canvas = null;
    public GameObject mainLayout = null;
    public GameObject instructionLayout = null;

    public void Update()
    {
        // fazer aqui os checks q impedem o menu de abrir
        if (Input.GetKeyDown(openMenuKey) && canvas.activeSelf == false)
        {
            OpenMenu();
        }
    }

    public void OpenMenu()
    {
        mainLayout.SetActive(true);
        instructionLayout.SetActive(false);
        canvas.SetActive(true);
    }
    public void CloseMenu()
    {
        canvas.SetActive(false);
        mainLayout.SetActive(false);
        instructionLayout.SetActive(false);
    }
    public void OpenInstructions()
    {
        mainLayout.SetActive(false);
        instructionLayout.SetActive(true);
    }
    public void CloseInstructions()
    {
        mainLayout.SetActive(true);
        instructionLayout.SetActive(false);
    }
}
