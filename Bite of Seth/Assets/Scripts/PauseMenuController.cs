using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public KeyCode openMenuKey = KeyCode.None;
    public GameObject canvas = null;
    public GameObject mainLayout = null;
    public GameObject instructionLayout = null;
    public GameObject settingsLayout = null;

    public SceneReference quitSceneToLoad = null;

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
        if (ServiceLocator.Get<GameManager>().loadingNewScene) {
            return;
        }
        mainLayout.SetActive(true);
        instructionLayout.SetActive(false);
        canvas.SetActive(true);
        ServiceLocator.Get<GameManager>().lockMovement += 1;
        ServiceLocator.Get<GameManager>().pause = true;
        ServiceLocator.Get<GameManager>().timerTrigger = false;
        Debug.Log("PAUSOU");
    }

    public void CloseMenu()
    {
        canvas.SetActive(false);
        mainLayout.SetActive(false);
        instructionLayout.SetActive(false);
        ServiceLocator.Get<GameManager>().lockMovement -= 1;
        Resume();
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

    public void OpenSettings()
    {
        mainLayout.SetActive(false);
        settingsLayout.SetActive(true);
    }

    public void CloseSettings()
    {
        mainLayout.SetActive(true);
        settingsLayout.SetActive(false);
    }

    public void UseCheckpoint()
    {
        FindObjectOfType<PlayerController>().UseCheckpoint();
        CloseMenu();
    }

    public void Quit(){
        Resume();
        CloseMenu();
        SceneManager.LoadScene(quitSceneToLoad.ScenePath);
    }

    public void Resume()
    {
        Invoke("ResumeGame", 0.01f);
    }

    private void ResumeGame()
    {
        ServiceLocator.Get<GameManager>().pause = false;
        ServiceLocator.Get<GameManager>().timerTrigger = true;
        Debug.Log("DESPAUSOU");
    }

}
