using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SceneReference FirstScene;

    private void Start()
    {
        
    }

    /*public void StartGame(){
        ServiceLocator.Get<GameManager>().StartNewGame(FirstScene);
        //SceneManager.LoadScene(FirstScene);
    }

    public void Continue()
    {
        SceneReference scene = ServiceLocator.Get<GameManager>().LoadGame();
        ServiceLocator.Get<SceneReferences>().GoToScene(scene);
    }*/

    public void QuitGame(){
        Debug.Log("Quit button pressed.");
        Application.Quit();
    }
}
