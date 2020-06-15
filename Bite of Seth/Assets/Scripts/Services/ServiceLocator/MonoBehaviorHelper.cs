using UnityEngine;

// this class intents to give the service locator access to monobehavior functions like Update()
public class MonoBehaviorHelper : MonoBehaviour
{
    private static MonoBehaviorHelper instance = null;
    void Start()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
        {
            Destroy(this);
        }        
    }
    void Update()
    {
        ServiceLocator.Update();
    }
}
