using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SceneReference FirstScene;

    public void StartGame(){
        SceneManager.LoadScene(FirstScene);
    }

    public void QuitGame(){
        Debug.Log("Quit button pressed.");
        Application.Quit();
    }
}
