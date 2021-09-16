using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueAnimHandler : MonoBehaviour
{
    Animator anim;

    void Awake() {
        anim = this.GetComponent<Animator>();
    }

    public void SetProximity(bool value) {
        anim.SetBool("player", value);
    }

    public void SetCommunication(bool value) {
        anim.SetBool("talk", value);
    }

    public void SetLock(bool value) {
        anim.SetBool("hold", value);
    }
}
