using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Manager/SceneReferences")]
public class SceneReferences : GameService {

    public List<SceneReference> scenesList;

    private int curSceneIndex = 0;

    private void OnGUI()
    {
        foreach (SceneReference sr in scenesList) {
            DisplayLevel(sr);
        }
    }

    public void DisplayLevel(SceneReference scene)
    {
        GUILayout.Label(new GUIContent("Scene name Path: " + scene));
        if (GUILayout.Button("Load " + scene)) {
            SceneManager.LoadScene(scene);
        }
    }

    public override void Start()
    {
        //curSceneIndex = SceneManager.GetActiveScene().buildIndex;
        curSceneIndex = 0;
        //Debug.Log(curSceneIndex);
    }

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("New Scene Loaded");
        SceneManager.SetActiveScene(scene);
        Debug.Log("Active Scene : " + SceneManager.GetActiveScene().path);

        //TO DO: CHECK IF THE SCENE IS A LEVEL SCENE
        if((ServiceLocator.Get<GameManager>()) != null){
            ServiceLocator.Get<GameManager>().SetNewLevel();
        }
        
    }

    public void GoToNextScene()
    {
        curSceneIndex++;
        //Load new scene
        //SceneManager.LoadScene(curSceneIndex, LoadSceneMode.Single);
        SceneManager.LoadScene(scenesList[curSceneIndex], LoadSceneMode.Single);
    }

    public void ReloadCurrentScene()
    {
        //Load current scene again
        //SceneManager.LoadScene(curSceneIndex, LoadSceneMode.Single);
        SceneManager.LoadScene(scenesList[curSceneIndex], LoadSceneMode.Single);
    }

}