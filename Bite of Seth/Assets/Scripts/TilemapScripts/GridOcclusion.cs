using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GridOcclusion : MonoBehaviour {
    public float radius = 10f;
    PlayerController player;
    Animator[] animators;
    Light2D[] lights;


    // Start is called before the first frame update
    void Start() {
        GameManager gm = ServiceLocator.Get<GameManager>();
        player = FindObjectOfType<PlayerController>();
        animators = GetComponentsInChildren<Animator>();
        lights = GetComponentsInChildren<Light2D>();
    }

    // Update is called once per frame
    void Update() {
        foreach (Animator obj in animators) {
            Vector2 objPos = new Vector2(obj.transform.position.x, obj.transform.position.y);
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
            obj.enabled = ((objPos - playerPos).magnitude <= radius);
        }

        foreach (Light2D obj in lights) {
            Vector2 objPos = new Vector2(obj.transform.position.x, obj.transform.position.y);
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
            obj.enabled = ((objPos - playerPos).magnitude <= radius);
        }
    }
}
