using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Light2DAsset : PlayableAsset {
    public Color color = Color.white;
    public float intensity = 1.0f;
    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner) {
        var playable = ScriptPlayable<Light2DBehaviour>.Create(graph);

        var light2DBehaviour = playable.GetBehaviour();
        light2DBehaviour.color = color;
        light2DBehaviour.intensity = intensity;

        return playable;
    }
}
