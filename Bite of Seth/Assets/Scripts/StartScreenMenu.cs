﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenMenu : MonoBehaviour {
    
    public bool canEnter = false, entered = false;
    public Animator animator;
    public GameObject menu;
    
    // Start is called before the first frame update
    void Start() {
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        animator.SetBool("entered", entered);


        if (canEnter && !entered && Input.GetKeyDown(KeyCode.Space)){
            entered = true;
        }
    }

    public void AllowCtrl(){
        canEnter = true;
    }

    public void BlockCtrl() {
        canEnter = false;
    }

    public void ShowMenu(){
        menu.SetActive(true);
    }
}
