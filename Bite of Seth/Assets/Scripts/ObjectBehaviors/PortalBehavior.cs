﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehavior : MonoBehaviour
{

    public GameObject otherSidePortalRef;
    public bool logicSide = false;
    
    //otp: object to transport
    private bool otpInRange;
    private GameObject otpRef;
    private TransportableBehavior otpTranspBeh;

    private bool playerInRange;
    private GameObject playerRef;
    private bool boulderInRange;
    private GameObject boulderRef;

    public bool canTransportBoulder = true;
    public bool canTransportPlayer = true;
    public Vector2 automaticSpawnDir = Vector2.right;

    public AudioObject sfx;
    private AudioManager am;

    public bool customCamSize = true;
    public float customSize = 4f;

    private bool active = true;
    public List<PortalEnergy> energies;

    // Start is called before the first frame update
    void Start()
    {
        am = ServiceLocator.Get<AudioManager>();
        if (energies.Count > 0) active = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!active) {
            active = true;
            foreach (PortalEnergy pe in energies) {
                active = active && pe.IsCollected();
            }
        }

        if(active && otpInRange && otpTranspBeh.CanTransport(otherSidePortalRef, canTransportBoulder, canTransportPlayer)) {
            if (sfx) {
                am.PlayAudio(sfx);
            }
            otpTranspBeh.TransportToPortal(otherSidePortalRef);
        }
    }

    public void ChangeObjectBehavior(TransportableBehavior tb)
    {
        tb.ChangeBehavior(GetComponent<PortalBehavior>());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TransportableBehavior tb = other.gameObject.GetComponent<TransportableBehavior>();
        if (tb != null) {
            otpInRange = true;
            otpRef = other.gameObject;
            otpTranspBeh = tb;
            if (canTransportPlayer) {
                PlayerController pc = otpRef.GetComponent<PlayerController>();
                if (pc && active) {
                    DialogueManager.instance.toggleInteractWithPortalAlert(true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(otpRef == other.gameObject) {
            if (canTransportPlayer) {
                PlayerController pc = otpRef.GetComponent<PlayerController>();
                if (pc) {
                    DialogueManager.instance.toggleInteractWithPortalAlert(false);
                }
            }
            otpInRange = false;
            otpRef = null;
            otpTranspBeh = null;
        }
    }
    
}
