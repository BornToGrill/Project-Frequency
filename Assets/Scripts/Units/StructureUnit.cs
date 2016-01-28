using UnityEngine;
using System;

public class StructureUnit : BaseUnit {

    public GameObject[] BuildableUnits;

    internal override int StackSize {
		get { return 1; }
		set {
		    if (value > MaxUnitStack)
		        throw new ArgumentOutOfRangeException(string.Format("Structure can only have {0} stacked units", MaxUnitStack));
		}
	}

	public override int GetCost (Environment environment){
		if (Owner.StartEnvironment != environment)
			return DiscountCost;
		return Cost;
	}

	public int GetCost (Environment environment, Player owner){
		if (owner.StartEnvironment != environment)
			return DiscountCost;
		return Cost;
	}

    public void CreateUnit(GameObject unit) {
        throw new NotImplementedException();
    }

	public override void DamageUnit(int damage, BaseUnit attacker) {
	    Health -= damage;
		if (Health <= 0) {
			GameObject.Destroy(gameObject);
			return;
		}
	}
}