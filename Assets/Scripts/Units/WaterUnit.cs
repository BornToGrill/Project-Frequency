﻿using System;
using System.Linq;
using UnityEngine;

public class WaterUnit : LandUnit {


    private GameObject _carryUnit;
    internal Environment[] _defaultEnvironments;

    internal override int StackSize {
        get { return 1; }
        set {
            if (value != 1)
                throw new InvalidOperationException("Only 1 water unit allowed per tile");
        }
    }

    internal GameObject CarryUnit {
        get {
            if (_carryUnit == null)
                _carryUnit = null;
            return _carryUnit;
        }
        set { _carryUnit = value; }
    }

    public override void Awake() {
        base.Awake();
        _defaultEnvironments = TraversableEnvironments;
    }

    public override bool CanMerge(BaseUnit unit) {
        if (CarryUnit == null)
            return unit.Owner == Owner;
        BaseUnit internalUnit = CarryUnit.GetComponent<BaseUnit>();
        return internalUnit.Owner == unit.Owner && CarryUnit.gameObject.name == unit.gameObject.name && internalUnit.StackSize + unit.StackSize <= internalUnit.MaxUnitStack;
    }

    public override void Merge(BaseUnit unit) {
        if (CarryUnit == null) {
            CarryUnit = unit.gameObject;
            CarryUnit.SetActive(false);
            TraversableEnvironments = _defaultEnvironments.Concat(unit.TraversableEnvironments).ToArray();
        }
        else {
            CarryUnit.GetComponent<BaseUnit>().StackSize += unit.StackSize;
            GameObject.Destroy(unit.gameObject);
        }

    }

    public void UnloadUnit(GameObject tile) {
        CarryUnit.SetActive(true);
		CarryUnit.GetComponent<LandUnit> ().ReloadAnimation ();
        CarryUnit.transform.position = gameObject.transform.position;
        CarryUnit = null;
        TraversableEnvironments = _defaultEnvironments;
    }

    public override void DamageUnit(int damage, BaseUnit attacker) {
		Health -= damage;
		if (Health > 0){
			_attackedBy = attacker;
			Animator anim = GetComponent<Animator> ();
			anim.Play ("Damage", 1);
			return;
		}
        Health = 0;
		GetComponent<SpriteRenderer> ().color = Color.white;
		Animator a = GetComponent<Animator> ();
		a.SetInteger ("StackSize", 0);

		if (CarryUnit != null) {
			GameObject.Destroy (CarryUnit);
			CarryUnit = null;
		}
        GameObject.Destroy(gameObject, 2f);
    }

	public override void Retaliate() {
		if (_attackedBy != null && CarryUnit != null)
			_attackedBy.DamageUnit (CarryUnit.GetComponent<LandUnit>()._stackDamage, null);
		_attackedBy = null;
	}
}