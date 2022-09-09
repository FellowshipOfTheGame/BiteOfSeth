using System.Collections;
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
    private List<PortalEnergy> energies;
    public bool seachEnergies = false;
    public SearchInRoom searchScript;
    public GameObject portalLock;

    // Start is called before the first frame update
    void Start()
    {
        if (seachEnergies) {
            energies = searchScript.GetListOfPortalEnergy();
            Debug.Log("Portais:");
            foreach (PortalEnergy pe in energies) Debug.Log(pe);
            Debug.Log("fim dos Portais");
        }
        am = ServiceLocator.Get<AudioManager>();
        if (energies != null && energies.Count > 0) active = false;
        if (portalLock != null) portalLock.SetActive(!active);
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) {
            int energyRemaining = 0;
            foreach (PortalEnergy pe in energies) {
                if (!pe.IsCollected()) 
                    energyRemaining++;
            }

            if (energyRemaining == 0) {
                active = true;
                if (portalLock != null) portalLock.SetActive(false);
            }
        }

        //if(active) sprite...

        if(active && otpInRange && otpTranspBeh.CanTransport(otherSidePortalRef, canTransportBoulder, canTransportPlayer)) {
            if (sfx) {
                am.PlayAudio(sfx);
            }
            otpTranspBeh.TransportToPortal(otherSidePortalRef);
        }
    }

    public void SetNewEnergy(PortalEnergy pe)
    {
        energies.Add(pe);
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
