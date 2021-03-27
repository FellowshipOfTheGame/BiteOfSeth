using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour
{

    public SceneReference NextLevelScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") 
        {
            if (NextLevelScene != null) {
                collision.gameObject.GetComponent<Movable>().StopSfx();
                ServiceLocator.Get<GameManager>().FromLevelGoToScene(NextLevelScene);
            } else {
                Debug.LogError("Sem referência para a próxima cena.");
            }
        }
    }
}
