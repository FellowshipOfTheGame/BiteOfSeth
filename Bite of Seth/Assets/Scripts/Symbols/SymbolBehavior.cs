using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolBehavior : CollectableBehavior {

    public Lore info;

    public override void Collect() {
        if (!collected) {
            LoreManager lm = ServiceLocator.Get<LoreManager>();
            lm.Learn(info);
        }
        base.Collect();
    }

    public override void Decollect() {
        if (collected) {
            LoreManager lm = ServiceLocator.Get<LoreManager>();
            lm.Forget(info);
        }
        base.Decollect();
    }
}
