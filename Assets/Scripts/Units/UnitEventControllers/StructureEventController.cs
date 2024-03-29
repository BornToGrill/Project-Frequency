﻿using UnityEngine;
using System.Linq;

public class StructureEventController : EventControllerBase {

    public Color BuildAllowedColor;
    public Color BuildNotAllowedColor;

    private bool _isBuilding;
    private GameObject _buildType;

    private TileController _hoveredTile;

    public override DeselectStatus OnSelected(GameObject ownTile) {
		UnitStats unitStats = GameObject.Find("UnitStats").GetComponent<UnitStats>();
		unitStats.Set (0, GetComponent<BaseUnit> ().Health);

        TileController thisTile = ownTile.GetComponent<TileController>();
        thisTile.GetComponent<SpriteRenderer>().color = SelfSelectedColor;

        if (!GetComponent<BaseUnit>().CurrentPlayerPredicate(thisTile))
            return DeselectStatus.None;
        ActionBarController actionBar = GameObject.Find("ActionBar").GetComponent<ActionBarController>();
		foreach (GameObject unit in GetComponent<StructureUnit>().BuildableUnits){
			BaseUnit unitComponent = unit.GetComponent<BaseUnit> ();
			Player unitOwner = gameObject.GetComponent<BaseUnit> ().Owner;
			if (unitComponent.GetCost(thisTile.Environment) > unitOwner.MoneyAmount || unitOwner.Moves <= 0)
				actionBar.AddButton(unit.name, CreateUnit, false, unitComponent.GetCost(thisTile.Environment), unitOwner.MoneyAmount);
			else
				actionBar.AddButton(unit.name, CreateUnit, true, unitComponent.GetCost(thisTile.Environment), unitOwner.MoneyAmount);
		}
            
        TileController[] directions = { thisTile.Left, thisTile.Up, thisTile.Right, thisTile.Down };
        foreach (TileController tile in directions.Where(x => x != null))
            ModifiedTiles.Add(tile);

        return DeselectStatus.None;
    }

    public override DeselectStatus OnClicked(GameObject ownTile, GameObject clickedTile) {
        // TODO: Refactor
        ownTile.GetComponent<TileController>().ResetSprite();
        if (!_isBuilding) {
            ModifiedTiles.Clear();
            return DeselectStatus.Both;
        }
        _isBuilding = false;
        foreach (TileController tile in ModifiedTiles)
            tile.ResetSprite();
        if (_hoveredTile != null)
            _hoveredTile.ResetSprite();

        TileController second = clickedTile.GetComponent<TileController>();
        if (!ModifiedTiles.Contains(second)) {
            ModifiedTiles.Clear();
            return DeselectStatus.Both;
        }
        ModifiedTiles.Clear();

        GameObject unit = (GameObject)Instantiate(_buildType, clickedTile.transform.position, Quaternion.identity);
        BaseUnit unitBase = unit.GetComponent<BaseUnit>();
        unitBase.Owner = GetComponent<BaseUnit>().Owner;
        if (!second.IsTraversable (unit)) {
			GameObject.Destroy (unit);
			return DeselectStatus.Both;
		}
		Animator anim = unitBase.GetComponent<Animator> ();
		if (anim != null)
			anim.Play ("Spawn", 1);

		unitBase.GetComponent<SpriteRenderer> ().color = unitBase.Owner.Color;
		unitBase.Owner.MoneyAmount -= unitBase.GetCost (ownTile.GetComponent<TileController> ().Environment);
		unitBase.Owner.Moves -= 1;

        StateController multiplayerController = GameObject.Find("Board").GetComponent<StateController>();
        if (multiplayerController != null)
            multiplayerController.ServerComs.Notify.CreateUnit(second, _buildType.name);

        _buildType = null;
        if (second.Unit != null) {
			if (second.IsTraversable (unit)) {
				if (((LandUnit)second.Unit).CanMerge (unitBase))
					((LandUnit)second.Unit).Merge (unitBase);
			}
            return DeselectStatus.Both;
        }
        else
            second.Unit = unitBase;

        return DeselectStatus.Both;
    }

    public void CreateUnit(string unitName) {
        _isBuilding = true;
        _buildType = GetComponent<StructureUnit>().BuildableUnits.Single(x => x.name == unitName);
        GameObject unitMock = (GameObject)Instantiate(_buildType, Vector3.zero, Quaternion.identity);
        unitMock.GetComponent<BaseUnit>().Owner = GetComponent<BaseUnit>().Owner;

        foreach (TileController tile in ModifiedTiles)
            if (tile.IsTraversable(unitMock))
                tile.GetComponent<SpriteRenderer>().color = BuildAllowedColor;
            else
                tile.GetComponent<SpriteRenderer>().color = BuildNotAllowedColor;
        GameObject.Destroy(unitMock);
    }

    public override void OnMouseEnter(GameObject ownTile, GameObject hoveredTile) {
        if (!_isBuilding || ownTile == hoveredTile)
            return;

        TileController hoverTile = hoveredTile.GetComponent<TileController>();
        if (!ModifiedTiles.Contains(hoverTile)) {
            _hoveredTile = hoverTile;
            _hoveredTile.GetComponent<SpriteRenderer>().color = BuildNotAllowedColor;
        }
    }

    public override void OnMouseLeave(GameObject ownTile, GameObject hoveredTile) {
        if (_hoveredTile != null && !ModifiedTiles.Contains(_hoveredTile))
            _hoveredTile.ResetSprite();
        _hoveredTile = null;
    }
}
