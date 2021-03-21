using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneBehavior : DialogueBehavior {

    public PlayableDirector timeline;

    public override void OnDialog() {
        //base.OnDialog();
        ts.playerInRange = true;
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    public override void OnEndDialog() {
        //base.OnEndDialog();
        ts.playerInRange = false;
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }
}
