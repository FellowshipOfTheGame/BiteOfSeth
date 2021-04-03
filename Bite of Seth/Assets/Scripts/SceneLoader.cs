using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public static SceneLoader instance;
    private void Awake()
    {
        if (instance != null) {
            Debug.LogError("Missing asset " + gameObject.name);
        } else {
            instance = this;
        }
    }

    public void LoadSceneAfterXSeconds(SceneReference scene, float X)
    {
        StartCoroutine(LoadScene(scene, X));
    }

    IEnumerator LoadScene(SceneReference scene, float X)
    {
        yield return new WaitForSeconds(X);

        ServiceLocator.Get<GameManager>().lockMovement--;

        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

}
