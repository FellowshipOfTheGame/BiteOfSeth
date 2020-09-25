using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour {

    public Material material;
    public Vector2 speed;

    public bool auto = true;

    bool move = false;

    // Start is called before the first frame update
    void Start(){
        move = false;
    }

    // Update is called once per frame
    void Update(){
        if (auto || move) {
            material.mainTextureOffset += speed * Time.deltaTime;
        }
    }

    public void TurnAndMove(float time) {
        speed = -speed;
        move = true;

        CancelInvoke();
        Invoke("Stop", time);
    }

    void Stop() {
        move = false;
    }
}
