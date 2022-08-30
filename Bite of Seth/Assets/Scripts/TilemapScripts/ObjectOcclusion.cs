using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOcclusion : MonoBehaviour {

    public float radius = 10f;
    CameraFollow cam;


    // Start is called before the first frame update
    void Start() {
        GameManager gm = ServiceLocator.Get<GameManager>();
        cam = FindObjectOfType<CameraFollow>();
    }

    // Update is called once per frame
    void Update() {
        foreach (Transform child in transform) {
            Vector2 pos = new Vector2(child.position.x, child.position.y);
            Vector2 camPos = new Vector2(cam.transform.position.x, cam.transform.position.y);

            if ((pos- camPos).magnitude > radius) {
                child.gameObject.SetActive(false);
            } else {
                child.gameObject.SetActive(true);
            }
        }
    }
}
