using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialController : MonoBehaviour {

    public Material material;
    public Vector2 speed;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += speed * Time.deltaTime;
    }
}
