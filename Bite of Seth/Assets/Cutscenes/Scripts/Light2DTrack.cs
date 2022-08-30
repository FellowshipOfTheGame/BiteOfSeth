using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Experimental.Rendering.Universal;

[TrackClipType(typeof(Light2DAsset))]
[TrackBindingType(typeof(Light2D))]
public class Light2DTrack : TrackAsset {
    //setting up the blend in and out of the clips
    

    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount) {
        return ScriptPlayable<Light2DMixerBehaviour>.Create(graph, inputCount);
    }
}
