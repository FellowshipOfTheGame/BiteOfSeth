using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Experimental.Rendering.Universal;

public class Light2DMixerBehaviour : PlayableBehaviour {

    Light2D trackBinding = null;
    Color defaultColor = Color.white;
    float defaultIntensity = 1.0f;

    bool firstFrameHappened = false;


    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData) {
        trackBinding = playerData as Light2D;

        if (!trackBinding)
            return;

        if (!firstFrameHappened){
            defaultColor = trackBinding.color;
            defaultIntensity = trackBinding.intensity;
            firstFrameHappened = true;
        }

        float finalIntensity = 0f;
        Color finalColor = Color.black;

        int inputCount = playable.GetInputCount (); //get the number of all clips on this track
        float inputSum = 0;

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<Light2DBehaviour> inputPlayable = (ScriptPlayable<Light2DBehaviour>)playable.GetInput(i);
            Light2DBehaviour input = inputPlayable.GetBehaviour();

            // Use the above variables to process each frame of this playable.
            finalIntensity += input.intensity * inputWeight;
            finalColor += input.color * inputWeight;
            inputSum += inputWeight;
        }
        Debug.Log(inputSum);
        //assign the result to the bound object
        if (inputSum == 0f) {
            trackBinding.intensity = defaultIntensity;
            trackBinding.color = defaultColor;
        } else {
            trackBinding.intensity = finalIntensity;
            trackBinding.color = finalColor;
        }

        //assign the result to the bound object
    }

    public override void OnPlayableDestroy (Playable playable) {
        firstFrameHappened = false;
        if(trackBinding == null)
            return;

        trackBinding.color = defaultColor;
        trackBinding.intensity = defaultIntensity;
    }
}
