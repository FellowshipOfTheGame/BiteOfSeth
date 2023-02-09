using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class MorphingStatue : MonoBehaviour
{

    [System.Serializable]
    public class Pair {
        public string name;
        public CharacterInfo character;
        public StatueAnimHandler handler;
        public SpriteRenderer art, eye, shine;
        public Light2D internalLight, externalLight;
        [HideInInspector] public Color internalLightColor, externalLightColor;

        public void SetVisibility(bool visibility) {
            art.gameObject.SetActive(true);

            art.enabled = visibility;
            eye.enabled = visibility;
            shine.enabled = visibility;

            if (visibility) {
                internalLight.color = internalLightColor;
                externalLight.color = externalLightColor;
            } else {
                internalLight.color = Color.black;
                externalLight.color = Color.black;
            }
        }
    }
    Animation transition;
    public Pair[] form;
    Pair lastForm = null, currentForm = null;

    void Awake() {
        foreach (Pair p in form) {
            p.internalLightColor = p.internalLight.color;
            p.externalLightColor = p.externalLight.color;
            p.SetVisibility(p == form[0]);
        }
        transition = this.GetComponent<Animation>();
    }

    void Update () {
        for (int i = 1; i < form.Length; i++) {
            form[i].handler.SetProximity( form[0].handler.GetProximity() );
            form[i].handler.SetCommunication( form[0].handler.GetCommunication() );
            form[i].handler.SetLock( form[0].handler.GetLock() );
        }
    }

    Pair FindForm(CharacterInfo character) {
        Debug.Log(character.characterName);
        foreach(Pair p in form) {
            if (p.character == character) 
                return p;
        }
        return null;
    }

    Pair GetActiveForm() {
        foreach(Pair p in form) {
            if (p.art.gameObject.activeInHierarchy) 
                return p;
        }

        return null;
    }

    public void CheckCharacter() {
        DialogueBase.Info info = DialogueManager.instance.currentInfo;
        if (info == null) return;

        currentForm = FindForm(info.character);
        if (currentForm == null) {
            return;
        }

        Debug.Log("Checking " + currentForm.name);

        if (lastForm == null) {
            lastForm = currentForm;
        } else if (currentForm != lastForm && !transition.isPlaying) {
            Debug.Log("CHANGE!");
            transition.Play();
        }
    }

    public void MorphInto(int index) {
        lastForm = GetActiveForm();

        if (index < form.Length && lastForm != form[index]) {
            currentForm = form[index];
            transition.Play();
        }
    }

    public void SwapCharacter() {
        if (lastForm != null) lastForm.SetVisibility(false);
        if (currentForm != null) currentForm.SetVisibility(true);
        lastForm = currentForm;
    }
}
