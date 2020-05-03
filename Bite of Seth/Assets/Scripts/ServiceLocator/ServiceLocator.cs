using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    public static GameManager gameManager = default;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void loadServiceList()
    {
        Debug.Log("Loading ServiceList to ServiceLocator");
        ServiceList sl = Resources.Load<ServiceList>("ServiceList");
        if (sl.gameManager == null)
        {
            Debug.LogWarning("ScoreManager not found in ServiceList, creating new");
            gameManager = ScriptableObject.CreateInstance<GameManager>();
        }
        else
        {
            gameManager = sl.gameManager;
        }        
    }
}
