using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Manager/SceneReferences")]
public class SceneReferences : GameService {

    public List<SceneReference> scenesList;
    private GameManager gm;
    private Animator sceneTransition;

    //private bool firstLoad;

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

        //Try to Load a New Level
        gm = ServiceLocator.Get<GameManager>();
        if (gm != null) {
            gm.lockMovement = 0;
            gm.TryToSetNewLevel();
        }
    
    }

    public void GoToNextScene()
    {
        curSceneIndex++;
        //Load new scene
        //SceneManager.LoadScene(curSceneIndex, LoadSceneMode.Single);
        if (curSceneIndex >= scenesList.Count) {
            curSceneIndex = 0;
        }
        SceneTransition st = FindObjectOfType<SceneTransition>();
        if (st) {
            st.StartAnimation();
        }
        SceneLoader.instance.LoadScene(scenesList[curSceneIndex]);

    }

    public void GoToScene(SceneReference scene)
    {
        //Load new scene
        if (scene != null) {
            SceneTransition st = FindObjectOfType<SceneTransition>();
            if (st) {
                st.StartAnimation();
            }
            SceneLoader.instance.LoadScene(scene);
        } else {
            Debug.LogError("Sem referência para a cena indicada.");
        }
        
    }

    public void ReloadCurrentScene()
    {
        //Load current scene again
        //SceneManager.LoadScene(curSceneIndex, LoadSceneMode.Single);
        SceneLoader.instance.LoadScene(scenesList[curSceneIndex]);
    }

    public SceneReference GetSceneReference(int id)
    {
        return scenesList[id];
    }

    public int GetSceneIndex(SceneReference scene)
    {
        int i = 0;
        foreach (SceneReference sr in scenesList){
            if (scene.ScenePath == sr.ScenePath) {
                return i;
            }
            i++;
        }
        return -1;
    }

    public string GetLevelName(int scene_id)
    {

        string[] aux = scenesList[scene_id].ScenePath.Split('/');
        aux = aux[aux.Length - 1].Split('.');
        return aux[0];

    }

}