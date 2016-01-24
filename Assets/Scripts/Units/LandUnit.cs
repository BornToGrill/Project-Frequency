using UnityEngine;

public class LandUnit : BaseUnit {

    public int Damage;
    public int Range;

    private int _stackSize;
    private int _stackHealth;
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
        _stackSize = Mathf.CeilToInt((float)_stackHealth / Health);
        _stackDamage = Damage * _stackSize;
    }
}
