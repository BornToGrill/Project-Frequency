using UnityEngine;
using System;
using System.Linq;

public class StructureUnit : BaseUnit {

    public GameObject[] BuildableUnits;


    private bool _isBuilding;
    private GameObject _buildType;

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

        ActionBarController actionBar = GameObject.Find("ActionBar").GetComponent<ActionBarController>();
        foreach (GameObject unit in BuildableUnits) {
            actionBar.AddButton(unit.name, CreateUnit); // TODO: TEMP
        }

        return DeselectStatus.None;
    }

    public override DeselectStatus OnSecondClicked(GameObject firstTile, GameObject secondTile) {
        if (!_isBuilding)
            return DeselectStatus.Both;
        //if secondTile is within building range
        //if secondTile has no unit
        GameObject unit = (GameObject)Instantiate(_buildType, secondTile.transform.position, new Quaternion());
        BaseUnit unitBase = unit.GetComponent<BaseUnit>();
        unitBase.Owner = Owner;
        secondTile.GetComponent<TileController>().Unit = unitBase;

        _isBuilding = false;
        _buildType = null;
        return DeselectStatus.Both;
    }

    public void CreateUnit(string unitName) {
        _isBuilding = true;
        _buildType = BuildableUnits.Single(x => x.name == unitName);
    }

    public override void OnMouseEnter(GameObject firstTile, GameObject secondTile) {
        throw new NotImplementedException();
    }

    public override void OnMouseLeave(GameObject firstTile, GameObject secondTile) {
        throw new NotImplementedException();
    }
}