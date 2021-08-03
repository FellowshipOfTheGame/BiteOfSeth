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

    public void LoadScene(SceneReference scene)
    {
        ServiceLocator.Get<GameManager>().loadingNewScene = true;
        ServiceLocator.Get<GameManager>().lockMovement++;
        StartCoroutine(LoadAsynchronously(scene));
    }

    IEnumerator LoadAsynchronously(SceneReference scene)
    {
        yield return new WaitForSeconds(1f);
        
        SceneTransition st = FindObjectOfType<SceneTransition>();

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            if (st) {
                st.UpdateProgressBar(progress);
            }
            yield return null;
        }

    }

}
