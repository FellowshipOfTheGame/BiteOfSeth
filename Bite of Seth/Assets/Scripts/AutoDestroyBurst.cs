using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyBurst : MonoBehaviour {

    ParticleSystem particle;

    // Start is called before the first frame update
    void Start() {
        particle = this.GetComponent<ParticleSystem>();
    }

    public void Play(){
        particle.Play();
        StartCoroutine( CountDown() );
    }

    private IEnumerator CountDown() {
        yield return new WaitForSeconds(particle.main.duration);
        Destroy(gameObject); 
    }
}
