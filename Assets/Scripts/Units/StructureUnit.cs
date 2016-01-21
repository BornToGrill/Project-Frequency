using UnityEngine;
using System;

public class StructureUnit : BaseUnit {

	private int _stackSize;

	internal override int StackSize {
		get { return _stackSize; }
		set {
		    if (value > MaxUnitStack)
		        throw new ArgumentOutOfRangeException(string.Format("Structure can only have {0} stacked units", MaxUnitStack));
		    _stackSize = value;
		}
	}

	public override int GetCost (Environment environment){
		if (Owner.StartEnvironment != environment)
			return DiscountCost;
		return Cost;
	}

    public void GetCreatableUnits(Board board) {
        throw new NotImplementedException(
            "Need the new Board class with up,down,left,right to get the surrounding tiles");
    }
    

    public void CreateUnit(GameObject unit) {
        throw new NotImplementedException();
    }

	public override void DamageUnit(int damage) {
	    Health -= damage;
		if (Health <= 0) {
			GameObject.Destroy(gameObject);
			return;
		}
	}

    public override DeselectStatus OnFirstSelected(GameObject firstTile) {
        throw new NotImplementedException();
    }

    public override DeselectStatus OnSecondClicked(GameObject firstTile, GameObject secondTile) {
        throw new NotImplementedException();
    }
}