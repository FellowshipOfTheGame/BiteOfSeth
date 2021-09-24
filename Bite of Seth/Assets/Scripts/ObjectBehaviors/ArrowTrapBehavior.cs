using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrapBehavior : MonoBehaviour
{

    public GameObject arrowPref;
    public Vector2 direction;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        Fire();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {
        Vector3 dir = Vector3.zero;
        dir.x = direction.normalized.x;
        dir.y = direction.normalized.y;
        GameObject arrow = Instantiate(arrowPref, transform.position + dir, Quaternion.identity);
        float angle = 0;
        if (dir.x == 1 && dir.y == 0) {
            angle = -90;
        } else if (dir.x == 0 && dir.y == -1) {
            angle = -180;
        } else if (dir.x == -1 && dir.y == 0) {
            angle = 90;
        }
        arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        arrow.GetComponent<ArrowBehavior>().StartMovement(direction, speed);
    }

}
