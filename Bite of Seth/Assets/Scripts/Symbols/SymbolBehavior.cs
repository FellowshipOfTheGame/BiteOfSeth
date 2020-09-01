using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolBehavior : MonoBehaviour {

    public Lore info;
    public ParticleSystem particles;
    public Animator anim;

    public SpriteRenderer symbol, shadow;

    private void Start() {
        symbol.sprite = info.symbol;
        shadow.sprite = info.shadow;
    }

    public void Collect() {
        this.enabled = false;
        anim.SetTrigger("collect");
    }

    public void Explode() {
        LoreManager lm = ServiceLocator.Get<LoreManager>();
        lm.Learn(info);
        particles.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController p = collision.gameObject.GetComponent<PlayerController>();
        if (p != null)
        {
            Collect();
        }
    }
}
