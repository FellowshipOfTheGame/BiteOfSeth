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

    // Start is called before the first frame update
    public virtual void Start()
    {
        mov = GetComponent<Movable>();
        lm = GetComponent<LogicMovable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual bool CanTransport(GameObject portal, bool canTransportBoulder)
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

    public virtual void ChangeBehavior(PortalBehavior portal)
    {
        //Generic changes
        if (portal.logicSide) {
            if (lm != null) {
                lm.enabled = true;
            }
        } else {
            if (lm != null) {
                lm.enabled = false;
            }
        }
    }

}
