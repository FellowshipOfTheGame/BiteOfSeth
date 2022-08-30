using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridOcclusion : MonoBehaviour {
    public float radius = 10f;
    CameraFollow cam;
    RoomBehavior[] rooms;


    // Start is called before the first frame update
    void Start() {
        rooms = GetComponentsInChildren<RoomBehavior>();
        GameManager gm = ServiceLocator.Get<GameManager>();
        cam = FindObjectOfType<CameraFollow>();
    }

    // Update is called once per frame
    void Update() {
        foreach (RoomBehavior room in rooms) {
            Vector2 roomPos = new Vector2(room.center.x, room.center.y);
            Vector2 camPos = new Vector2(cam.transform.position.x, cam.transform.position.y);

            if ((roomPos- camPos).magnitude > radius) {
                room.gameObject.SetActive(false);
            } else {
                room.gameObject.SetActive(true);
            }
        }
    }
}
