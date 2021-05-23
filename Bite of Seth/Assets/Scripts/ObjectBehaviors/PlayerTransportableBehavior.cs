using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransportableBehavior : TransportableBehavior
{

    private PlayerController pc;
    private AudioManager am;

    public override void Start()
    {
        base.Start();
        pc = GetComponent<PlayerController>();
        am = ServiceLocator.Get<AudioManager>();
    }

    public override bool CanTransport(GameObject portal, bool canTransportBoulder, bool canTransportPlayer)
    {
        return (base.CanTransport(portal, canTransportBoulder, canTransportPlayer) && canTransportPlayer);
    }

    public override void ChangeBehavior(PortalBehavior portal)
    {
        base.ChangeBehavior(portal);
        //Specific changes to player
        if (portal && portal.logicSide) {
            pc.ChangeToLogicSpeed();
            am.ChangeToLogicBGM();
            if (portal.customCamSize) {
                ServiceLocator.Get<GameManager>().changeCameraToCustomSize(portal.customSize);
            }
        } else {
            pc.ChangeToNormalSpeed();
            am.ChangeToDefaultBGM();
            if (!portal || portal.customCamSize) {
                ServiceLocator.Get<GameManager>().changeCameraToDefaultSize();
            }
        }
    }

}
