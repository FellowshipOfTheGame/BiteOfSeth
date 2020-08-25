using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    private static Dictionary<string, GameService> services = new Dictionary<string, GameService>();
    private static GameObject monoHelperObject = null;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void SetupServiceLocator()
    {
        // instantiate the monoBehaviorHelper singleton
        monoHelperObject = new GameObject("ServiceLocator Helper");
        monoHelperObject.AddComponent<MonoBehaviorHelper>();

        // loads service list
        Debug.Log("Loading ServiceList to ServiceLocator");
        ServiceList serviceList = Resources.Load<ServiceList>("ServiceList");
        foreach(GameService service in serviceList.services)
        {
            Register(service);
        }

        //Do the job for the first scene loaded on SceneReferences
        Get<GameManager>().TryToSetNewLevel();
    }

    // this function should only be called by the monoBehaviorHelper singleton
    public static void Update()
    {
        // propagates the update to the individual services registered
        foreach(KeyValuePair<string, GameService> pair in services)
        {
            pair.Value.Update();
        }
    }

    public static T Get<T>() where T : GameService
    {
        string key = typeof(T).Name;
        if (key == typeof(GameService).Name)
        {
            Debug.LogError($"Cannot get service of abstract class 'GameService'.");
            return null;
        }
        if (!services.ContainsKey(key))
        {
            Debug.LogError($"Service of type {key} not registered");
            return null; // instantiate and register default gameService

        }
        return (T)services[key];
    }

    public static void Register<T>(T service) where T : GameService
    {
        string key = service.GetType().ToString();
        if (services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to register service of type {key} which is already registered with the {services[key].name}.");
            return;
        }
        services.Add(key, service);
        service.Start();
    }

    public static void Unregister<T>() where T : GameService
    {
        string key = typeof(T).Name;
        if (key == typeof(GameService).Name)
        {
            Debug.LogError($"Cannot unregister service of abstract class 'GameService'.");
            return;
        }
        if (!services.ContainsKey(key))
        {
            Debug.LogError($"Attempted to unregister a not registered service of type {key}.");
            return;
        }
        services.Remove(key);
    }

    public static T AddComponentToHelperObject<T>() where T : Component
    {
        T component = monoHelperObject.AddComponent<T>() as T;
        return component;
    }
}
