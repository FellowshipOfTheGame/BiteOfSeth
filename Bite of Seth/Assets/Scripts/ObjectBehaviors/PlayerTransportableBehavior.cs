using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransportableBehavior : TransportableBehavior
{

    private PlayerController pc;

    public override void Start()
    {
        base.Start();
        pc = GetComponent<PlayerController>();
    }

    public override bool CanTransport(GameObject portal, bool canTransportBoulder, bool canTransportPlayer)
    {
        return (base.CanTransport(portal, canTransportBoulder, canTransportPlayer) && canTransportPlayer);
    }

    public override void ChangeBehavior(PortalBehavior portal)
    {
        base.ChangeBehavior(portal);
        //Specific changes to player
        if (portal.logicSide) {
            pc.ChangeToLogicSpeed();
        } else {
            pc.ChangeToNormalSpeed();
        }
    }

}
