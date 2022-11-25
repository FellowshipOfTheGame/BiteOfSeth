using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolBehavior : MonoBehaviour {

    public Lore info;
    public int index;
    public int level;
    public ParticleSystem particles;
    public Animator anim;

    public SpriteRenderer symbol, shadow;

    private bool collected = false;

    private void Start() {
        symbol.sprite = info.symbol;
        shadow.sprite = info.shadow;
        collected = false;
    }

    public void Collect() {
        this.enabled = false;
        anim.SetTrigger("collect");
        if (!collected) {
            collected = true;
            ServiceLocator.Get<GameManager>().AddLevelPiecesOfLore(1);
        }
    }

    public void Explode() {
        LoreManager lm = ServiceLocator.Get<LoreManager>();
        //lm.Learn(info);
        lm.SetCollectedLore(this, true);
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
