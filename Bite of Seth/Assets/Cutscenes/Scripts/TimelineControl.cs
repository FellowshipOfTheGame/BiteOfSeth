using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineControl : MonoBehaviour {

    PlayableDirector timeline;

    [HideInInspector] public bool isPlaying;

    void Awake() {
        timeline = this.GetComponent<PlayableDirector>();
        isPlaying = timeline.playOnAwake;
    }

    public void Play() {
        timeline.Play();
        isPlaying = true;
        ServiceLocator.Get<GameManager>().StopPlayerControls();
    }

    public void Stop() {
        timeline.Stop();
        isPlaying = false;
        ServiceLocator.Get<GameManager>().ResumePlayerControls();
    }

    public void Pause() {
        if (isPlaying) timeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    public void Resume() {
        if (isPlaying) timeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }
}
