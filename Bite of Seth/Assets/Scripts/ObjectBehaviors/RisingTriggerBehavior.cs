using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingTriggerBehavior : MonoBehaviour
{

    [System.Serializable]
    public struct Trigger {
        public RisingHazardBehavior risingHazard;
        public bool expand;
    }

    public List<Trigger> triggers;

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
        if(collision.tag == "Player") {
            foreach (Trigger t in triggers) {
                if (t.expand) {
                    t.risingHazard.StartExpanding();
                } else {
                    t.risingHazard.StartShrinking();
                }
            }
        }
    }

}