using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class StructureUnit : BaseUnit {

    public GameObject[] BuildableUnits;

    public Color BuildAllowedColor;
    public Color BuildNotAllowedColor;

    private bool _isBuilding;
    private GameObject _buildType;

    private List<TileController> _surroundingTiles = new List<TileController>();
    private TileController _hoveredTile;

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
        foreach (GameObject unit in BuildableUnits)
            actionBar.AddButton(unit.name, CreateUnit);

        TileController thisTile = firstTile.GetComponent<TileController>();
        thisTile.GetComponent<SpriteRenderer>().color = SelfSelectedColor;

        TileController[] directions = { thisTile.Left, thisTile.Up, thisTile.Right, thisTile.Down };
        foreach (TileController tile in directions.Where(x => x != null))
            _surroundingTiles.Add(tile);

        return DeselectStatus.None;
    }

    public override DeselectStatus OnSecondClicked(GameObject firstTile, GameObject secondTile) {
        // TODO: Refactor
        firstTile.GetComponent<TileController>().ResetSprite();
        if (!_isBuilding)
            return DeselectStatus.Both;
        _isBuilding = false;
        foreach (TileController tile in _surroundingTiles)
            tile.ResetSprite();

        if (_hoveredTile != null)
            _hoveredTile.ResetSprite();

        // TODO: Check if you've got enough moves remaining.
        TileController second = secondTile.GetComponent<TileController>();
        if (!_surroundingTiles.Contains(second)) {
            _surroundingTiles.Clear();
            return DeselectStatus.Both;
        }
        _surroundingTiles.Clear();

        GameObject unit = (GameObject)Instantiate(_buildType, secondTile.transform.position, Quaternion.identity);
        BaseUnit unitBase = unit.GetComponent<BaseUnit>();
        unitBase.Owner = Owner;
        _buildType = null;
        if (second.Unit != null) {
            if (second.IsTraversable(unit)) {
                second.Unit.StackSize++;
            }
            GameObject.Destroy(unit);
            return DeselectStatus.Both;
        }
        else
            second.Unit = unitBase;

        return DeselectStatus.Both;
    }

    public void CreateUnit(string unitName) {
        _isBuilding = true;
        _buildType = BuildableUnits.Single(x => x.name == unitName);
        GameObject unitMock = (GameObject)Instantiate(_buildType, Vector3.zero, Quaternion.identity);
        unitMock.GetComponent<BaseUnit>().Owner = this.Owner;

        foreach(TileController tile in _surroundingTiles)
            if (tile.IsTraversable(unitMock))
                tile.GetComponent<SpriteRenderer>().color = BuildAllowedColor;
            else
                tile.GetComponent<SpriteRenderer>().color = BuildNotAllowedColor;
        GameObject.Destroy(unitMock);
    }

    public override void OnMouseEnter(GameObject firstTile, GameObject secondTile) {
        if (!_isBuilding || firstTile == secondTile)
            return;

        TileController hoverTile = secondTile.GetComponent<TileController>();
        if (!_surroundingTiles.Contains(hoverTile)) {
            _hoveredTile = hoverTile;
            _hoveredTile.GetComponent<SpriteRenderer>().color = BuildNotAllowedColor;
        }
    }

    public override void OnMouseLeave(GameObject firstTile, GameObject secondTile) {
        if (_hoveredTile != null && !_surroundingTiles.Contains(_hoveredTile))
            _hoveredTile.ResetSprite();
        _hoveredTile = null;
    }
}