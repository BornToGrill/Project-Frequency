using System;
using UnityEngine;
using System.Collections.Generic;

public class WaterUnit : BaseUnit {

    private int _stackSize;

    internal override int StackSize {
        get { return _stackSize; }
        set {
            if (value != 1)
                throw new InvalidOperationException("Only 1 water unit allowed per tile");
            _stackSize = value;
        }
    }

    internal BaseUnit CarryUnit;

    public void LoadUnit(BaseUnit unit) {
        CarryUnit = unit;
    }

    public void UnloadUnit() {
        CarryUnit = null;
    }

    public override void DamageUnit(int damage) {
        Health -= damage;
        if (Health > 0)
            return;
        Health = 0;
        GameObject.Destroy(gameObject);
        // TODO: Death check.
    }
}
