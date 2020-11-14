using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportableBehavior : MonoBehaviour
{

    //If the transport is automatic, then the object is instantly tranported. 
    //Otherwise, the object(player) needs an input to be transported;
    public bool isAutomatic = true;

    private LogicMovable lm;
    private Movable mov;
    private FallBehavior fb;
    private RollDelay rd;
    private PushableBehavior pb;
    private PlayerController pc;

    public Vector2 spawnDir = Vector2.right;

    // Start is called before the first frame update
    void Start()
    {
        mov = GetComponent<Movable>();
        lm = GetComponent<LogicMovable>();
        fb = GetComponent<FallBehavior>();
        rd = GetComponent<RollDelay>();
        pb = GetComponent<PushableBehavior>();
        pc = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanTransport(GameObject portal)
    {
        //Verificar se pode se teletransportar
        if (mov == null) {
            return false;
        }
        if (isAutomatic) {
            //Adicionar lógica de spawnar mais de 1 objeto se precisar
            spawnDir = portal.GetComponent<PortalBehavior>().automaticSpawnDir;
            return ( !(mov.isMoving) );
        } else {
            return ( Input.GetKeyDown(KeyCode.T) && !(mov.isMoving) );
        }
    }

    public void TransportToPortal(GameObject portal)
    {
        lm.GetComponent<LogicMovable>().enabled = false;
        mov.rigidbody.position = (Vector3)GridNav.WorldToGridPosition((Vector2)portal.transform.position + spawnDir);
        mov.StoppedMoving();
        mov.isMoving = false;
        ChangeBehavior(portal.GetComponent<PortalBehavior>());
    }

    public void ChangeBehavior(PortalBehavior portal)
    {
        if (portal.logicSide) {
            if (lm != null) {
                lm.enabled = true;
            }
            if (fb != null) {
                fb.enabled = false;
            }
            if (rd != null) {
                rd.enabled = false;
            }
            if (pb != null) {
                pb.ChangeToLogicSpeed();
            }
            if (pc != null) {
                pc.ChangeToLogicSpeed();
            }
        } else {
            if (lm != null) {
                lm.enabled = false;
            }
            if (fb != null) {
                fb.enabled = true;
            }
            if (rd != null) {
                rd.enabled = true;
            }
            if (pb != null) {
                pb.ChangeToNormalSpeed();
            }
            if (pc != null) {
                pc.ChangeToNormalSpeed();
            }
        }
    }

}
