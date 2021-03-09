using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderTransportableBehavior : TransportableBehavior
{
    private FallBehavior fb;
    private RollDelay rd;
    private PushableBehavior pb;

    private PortalBehavior _portal;

    public override void Start()
    {
        base.Start();
        fb = GetComponent<FallBehavior>();
        rd = GetComponent<RollDelay>();
        pb = GetComponent<PushableBehavior>();
    }

    public override bool CanTransport(GameObject portal, bool canTransportBoulder, bool canTransportPlayer)
    {
        return (base.CanTransport(portal, canTransportBoulder, canTransportPlayer) && canTransportBoulder);
    }

    public override void ChangeBehavior(PortalBehavior portal)
    {
        base.ChangeBehavior(portal);
        //Specific changes to player
        if (portal.logicSide) {
            fb.isRolling = false;
            fb.rollingDirection = 0f;
            fb.animator.SetBool("Roll", false);
            fb.animator.SetFloat("Direction", 0.0f);
            fb.enabled = false;
            rd.enabled = false;
            pb.ChangeToLogicSpeed();
        } else {
            rd.enabled = true;
            fb.enabled = true;
            pb.ChangeToNormalSpeed();
        }
    }

}
