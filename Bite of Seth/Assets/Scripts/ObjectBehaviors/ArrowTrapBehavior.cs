using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrapBehavior : MonoBehaviour
{

    public GameObject arrowPref;
    public Vector2 direction;
    public float speed;

    public float shootTime = 3f;

    public LayerMask arrowBlockersMask;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Fire", shootTime);
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

        List<GameObject> blockers = GridNav.GetObjectsInPath(transform.position, dir, arrowBlockersMask, gameObject);
        if (blockers.Count > 0) {
            Invoke("Fire", shootTime);
            return;
        }

        GameObject arrow = Instantiate(arrowPref, transform.position, Quaternion.identity);

        float angle = 90;
        if (dir.x == 1 && dir.y == 0) {
            angle = 0;
        } else if (dir.x == 0 && dir.y == -1) {
            angle = -90;
        } else if (dir.x == -1 && dir.y == 0) {
            angle = 180;
        }
        arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        arrow.GetComponent<ArrowBehavior>().StartMovement(direction, speed);

        Invoke("Fire", shootTime);

    }

}
