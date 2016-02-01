using UnityEngine;
using System;
using System.Collections;

public class LandUnit : BaseUnit {

    public int Damage;
    public int Range;

    private int _stackSize;
    internal int _stackHealth;
    internal int _stackDamage;

    internal override int StackSize {
        get { return _stackSize; }
        set {
            int diff = Mathf.Abs(value - _stackSize);
            _stackHealth += diff * Health;
            _stackDamage += diff * Damage;
            _stackSize = value;
			Animator anim = GetComponent<Animator> ();
			anim.SetInteger ("StackSize", _stackSize);
        }
    }

    public virtual bool CanMerge(BaseUnit unit) {
        return Owner == unit.Owner && StackSize + unit.StackSize <= MaxUnitStack && gameObject.name == unit.gameObject.name;
    }

    public virtual void Merge(BaseUnit unit) {
		StackSize += unit.StackSize;
        GameObject.Destroy(unit.gameObject);
    }

	public virtual void ReloadAnimation() {
		Animator anim = GetComponent<Animator> ();
		anim.SetInteger ("StackSize", _stackSize);
		anim.Update (Time.deltaTime);
	}

    public void Attack(BaseUnit unit) {
        if (unit.Owner != this.Owner)
            unit.DamageUnit(_stackDamage, this);
    }

    public override void DamageUnit(int damage, BaseUnit attacker) {
        _stackHealth -= damage;
		Animator anim = GetComponent<Animator> ();
		anim.Play ("Damage", 1);
        if (_stackHealth <= 0) {
			StackSize = 0;
			GameObject.Destroy (gameObject, 2f); // Insurance in case it's not destroyed by explosion animation.
            return;
        }
        _stackSize = Mathf.CeilToInt((float)_stackHealth / Health);
        _stackDamage = Damage * _stackSize;
        ReloadAnimation();
        if (attacker != null)
        {
            attacker.DamageUnit(_stackDamage, null);
        }
    }

	public void DestroyUnit() {
		GameObject.Destroy (gameObject);
	}


}