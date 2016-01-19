using UnityEngine;
using System.Collections.Generic;

public class WaterUnit : BaseUnit {

    internal BaseUnit CarryUnit;

    public void LoadUnit(BaseUnit unit) {
        CarryUnit = unit;
    }

    public void UnloadUnit() {
        CarryUnit = null;
    }

    public override void DamageUnit(int damage) {
        Health -= damage;
        // TODO: Death check.
    }
}
