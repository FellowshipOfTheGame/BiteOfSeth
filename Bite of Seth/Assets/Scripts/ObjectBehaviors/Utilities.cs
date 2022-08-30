using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Utilities : MonoBehaviour {

    [System.Serializable]
    public class ActionList {
        public string name;
        public UnityEvent[] actions;
    }

    public ActionList[] list;

    public int utility, action;
    public void SelectUtility(int index) { utility = index; }

    public void SelectAction(int index) { action = index; }

    public void DoAction() {
        if (utility < list.Length && action < list[utility].actions.Length) {
            list[utility].actions[action].Invoke();
        } else {
            Debug.Log("Action or Utility out of index");
        }
    }

    public void MoveTo(Transform destiny) {
        this.transform.position = destiny.position;
    }
}
