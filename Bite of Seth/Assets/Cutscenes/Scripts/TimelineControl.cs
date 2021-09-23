using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineControl : MonoBehaviour {

    PlayableDirector timeline;

    void Awake() {
        timeline = this.GetComponent<PlayableDirector>();
    }

    public void Pause() {
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    public void Resume() {
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
        Debug.Log("Continua");
    }
}
