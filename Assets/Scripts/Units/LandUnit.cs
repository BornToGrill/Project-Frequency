﻿using UnityEngine;
using System.Collections;

public class LandUnit : BaseUnit {

    public int Damage;
    public int Range;

    private int _stackSize;
    private int _stackHealth;
    private int _stackDamage;

    internal int StackSize {
        get { return _stackSize; }
        set {
            int diff = Mathf.Abs(value - _stackSize);
            _stackHealth += diff * Health;
            _stackDamage += diff * Damage;
            _stackSize = value;
        }
    }

    public bool CanAttack(BaseUnit unit) {
        return unit.Owner != this.Owner;
    }

    public void Attack(BaseUnit unit) {
        if (unit.Owner != this.Owner)
            unit.DamageUnit(_stackDamage);
    }

    public override void DamageUnit(int damage) {
        _stackHealth -= damage;
        if (_stackHealth <= 0) {
            GameObject.Destroy(gameObject);
            return;
        }
        _stackSize = Mathf.CeilToInt(_stackHealth / Health);
        _stackDamage = Damage * _stackSize;
        // TODO: Death check for all and individual units in stack.
    }
}
