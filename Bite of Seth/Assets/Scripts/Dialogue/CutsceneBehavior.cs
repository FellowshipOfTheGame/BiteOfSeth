using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneBehavior : DialogueBehavior {

    public PlayableDirector timeline;
    public float holdTime = 1f;
    float startHoldTime = 0f;
    bool quitting = false;

    public SceneReference NextLevelScene;

    private void Update() {
        
        if (!ts.TryToDialogue() && Input.anyKeyDown) {
            if (DialogueManager.instance.isDialogueActive == false || !Input.GetKeyDown(KeyCode.E)) {
                DialogueManager.instance.toggleInteractWithCutsceneAlert(true);
                CancelInvoke();
                Invoke("EraseWarning", 1.5f);
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                startHoldTime = 0f;
                quitting = true;
                CancelInvoke();
            }
        }

        if (quitting) {
            if (Input.GetKeyUp(KeyCode.Escape)) {
                quitting = false;
                EraseWarning();
            }
            if (startHoldTime >= holdTime) ExitCutscene();
            startHoldTime += Time.deltaTime;
        }
    }

    void EraseWarning() {
        DialogueManager.instance.toggleInteractWithCutsceneAlert(false);
    }

    public void ExitCutscene() {
        CancelInvoke();
        quitting = false;

        if (NextLevelScene != null) {
            ServiceLocator.Get<GameManager>().FromCutsceneGoToScene(NextLevelScene);
        } else {
            Debug.LogError("Sem referência para a próxima cena.");
        }
    }

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
