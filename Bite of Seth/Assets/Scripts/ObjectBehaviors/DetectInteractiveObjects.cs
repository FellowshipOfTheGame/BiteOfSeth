using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectInteractiveObjects : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ExplodeBehavior eb = collision.gameObject.GetComponent<ExplodeBehavior>();
        if (eb != null) {
            DialogueManager.instance.toggleInteractWithDynamiteAlert(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExplodeBehavior eb = collision.gameObject.GetComponent<ExplodeBehavior>();
        if (eb != null) {
            DialogueManager.instance.toggleInteractWithDynamiteAlert(false);
        }
    }


}
