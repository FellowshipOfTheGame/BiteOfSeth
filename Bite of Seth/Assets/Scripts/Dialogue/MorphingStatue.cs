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

        public void SetVisibility(bool visibility) {
            art.gameObject.SetActive(true);

            art.enabled = visibility;
            eye.enabled = visibility;
            shine.enabled = visibility;
            internalLight.enabled = visibility;
            externalLight.enabled = visibility;
        }
    }
    Animation transition;
    public Pair[] form;
    Pair lastForm = null, currentForm = null;

    void Awake() {
        if (form.Length > 0) {
            form[0].SetVisibility(true);
            foreach(Pair p in form) {
                if (p != form[0]) p.SetVisibility(false);
            }
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
