using UnityEngine;

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
        }
    }

    public virtual bool CanMerge(BaseUnit unit) {
        return Owner == unit.Owner && StackSize + unit.StackSize <= MaxUnitStack && gameObject.name == unit.gameObject.name;
    }

    public virtual void Merge(BaseUnit unit) {
		StackSize += unit.StackSize;
		Animator anim = GetComponent<Animator> ();
		anim.SetInteger ("StackSize", StackSize);
        GameObject.Destroy(unit.gameObject);
    }

    public void Attack(BaseUnit unit) {
        if (unit.Owner != this.Owner)
            unit.DamageUnit(_stackDamage, this);
    }

    public override void DamageUnit(int damage, BaseUnit attacker) {

        _stackHealth -= damage;
        if (_stackHealth <= 0) {
            GameObject.Destroy(gameObject);
            return;
        }
        _stackSize = Mathf.CeilToInt((float)_stackHealth / Health);
        _stackDamage = Damage * _stackSize;
        if (attacker != null)
        {
            attacker.DamageUnit(_stackDamage, null);
        }
        
    }
}