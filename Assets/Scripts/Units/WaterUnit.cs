using System;
using UnityEngine;

public class WaterUnit : LandUnit {



    internal override int StackSize {
        get { return 1; }
        set {
            if (value != 1)
                throw new InvalidOperationException("Only 1 water unit allowed per tile");
        }
    }

    internal BaseUnit CarryUnit;


    public override bool CanMerge(BaseUnit unit) {
        return CarryUnit == null || (CarryUnit.Owner == unit.Owner && CarryUnit.gameObject.name == unit.gameObject.name);
    }

    public override void Merge(BaseUnit unit) {
        if (CarryUnit == null)
            CarryUnit = unit;
        else
            CarryUnit.StackSize += unit.StackSize;
        GameObject.Destroy(unit.gameObject);
    }

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
    }
}
