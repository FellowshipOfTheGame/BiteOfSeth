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

    public Vector2 spawnDir = Vector2.right;

    public bool logicBehavior = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        mov = GetComponent<Movable>();
        lm = GetComponent<LogicMovable>();
        logicBehavior = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual bool CanTransport(GameObject portal, bool canTransportBoulder, bool canTransportPlayer)
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
            return ( Input.GetKeyDown(KeyCode.E) && !(mov.isMoving) && ServiceLocator.Get<GameManager>().lockMovement == 0);
        }
    }

    public void TransportToPortal(GameObject portal)
    {
        //Debug.Log("PARA");
        mov.isMoving = false;

        //Debug.Log("MUDA");
        ChangeBehavior(portal.GetComponent<PortalBehavior>());

        //Debug.Log("TELETRANSPORTA");
        mov.rigidbody.position = (Vector3)GridNav.WorldToGridPosition((Vector2)portal.transform.position + spawnDir);

        //Debug.Log("COMPORTAMENTO PÓS-TELETRANSPORTE");
        if (portal.GetComponent<PortalBehavior>().logicSide) {
            PushableBehavior pbh = GetComponent<PushableBehavior>();
            if (pbh) {
                pbh.Push(spawnDir);
            }
        } else {
            mov.StoppedMoving();
        }

    }

    public virtual void ChangeBehavior(PortalBehavior portal)
    {
        //Generic changes
        if (portal && portal.logicSide) {
            if (lm != null) {
                //lm.enabled = true;
                lm.EnableLogicMovement();
            }
            logicBehavior = true;
        } else {
            if (lm != null) {
                //lm.enabled = false;
                lm.DisableLogicMovement();
            }
            logicBehavior = false;
        }
    }

}
