using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueAnimHandler : MonoBehaviour
{
    Animator anim;

    bool player, talk, hold;

    void Awake() {
        anim = this.GetComponent<Animator>();
        player = false;
        talk = false;
        hold = false;
    }

    public bool GetProximity() { return player;}

    public void SetProximity(bool value) {
        anim.SetBool("player", value);
        player = value;
    }

    public bool GetCommunication() { return talk;}

    public void SetCommunication(bool value) {
        anim.SetBool("talk", value);
        talk = value;
    }
    public bool GetLock() { return hold; }
    public void SetLock(bool value) {
        anim.SetBool("hold", value);
        hold = value;
    }
}
