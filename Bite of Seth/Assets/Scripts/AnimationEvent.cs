using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour {
    [System.Serializable]
    public class BiteEvent {
        public string name;
        public UnityEvent commands;
    }

    public BiteEvent[] events;

    public void Call(string eventName) {
        foreach (BiteEvent be in events) {
            if (be.name == eventName) {
                be.commands.Invoke();
                return;
            }
        }

        Debug.Log("Event " + eventName + " not found...");
    }

    public void DestroyObject(GameObject target) {
        Destroy(target);
    }
}
