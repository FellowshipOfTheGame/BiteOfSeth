using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SceneReference FirstScene;
    public GameObject continueButton;
    public GameObject resetButton;

    private void Start()
    {
        if (ServiceLocator.Get<GameManager>().HasSaveFile()) {
            continueButton.SetActive(true);
            resetButton.SetActive(true);
        }   
    }

    public void StartGame(){
        ServiceLocator.Get<GameManager>().StartNewGame(FirstScene);
        //SceneManager.LoadScene(FirstScene);
    }

    public void Continue()
    {
        SceneReference scene = ServiceLocator.Get<GameManager>().LoadGame();
        ServiceLocator.Get<SceneReferences>().GoToScene(scene);
    }

    public void ResetSaveData()
    {
        ServiceLocator.Get<GameManager>().DeleteSaveFile();
        ServiceLocator.Get<SceneReferences>().ReloadCurrentScene();
    }

    public void QuitGame(){
        Debug.Log("Quit button pressed.");
        Application.Quit();
    }
}
