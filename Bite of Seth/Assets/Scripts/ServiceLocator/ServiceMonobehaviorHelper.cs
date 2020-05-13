using UnityEngine;

// this class intents to give the service locator access to monobehavior functions like Update()
public class ServiceMonobehaviorHelper : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        ServiceLocator.Update();
    }
}
