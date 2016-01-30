using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class SoldierEventController : LandUnitEventController {


    //
    // See documentation : http://goo.gl/dMJsp7
    //


    public Color BuildAllowedColor;
    public Color BuildNotAllowedColor;

    private bool _isBuilding;
    private GameObject _buildType;
    private TileController _hoveredTile;
    private List<TileController> _surroundingTiles = new List<TileController>();

    public override DeselectStatus OnSelected(GameObject ownTile) {
        TileController thisTile = ownTile.GetComponent<TileController>();
        if (!thisTile.Unit.Owner.IsCurrentPlayer)
            return base.OnSelected(ownTile);

        ActionBarController actionBar = GameObject.Find("ActionBar").GetComponent<ActionBarController>();
		foreach (GameObject structure in GetComponent<SoldierUnit>().BuildableStructures) {
			StructureUnit building = structure.GetComponent<StructureUnit> ();
			Player owner = GetComponent<BaseUnit> ().Owner;
			if ( building.GetCost (thisTile.Environment, owner) > owner.MoneyAmount || owner.Moves <= 0) {
				actionBar.AddButton (structure.name, CreateStructure, false, building.GetCost(thisTile.Environment), owner.MoneyAmount);
			} else {
				actionBar.AddButton (structure.name, CreateStructure, true, building.GetCost(thisTile.Environment), owner.MoneyAmount);
			}
		}
        

        TileController[] directions = { thisTile.Left, thisTile.Up, thisTile.Right, thisTile.Down };
        foreach (TileController tile in directions.Where(x => x != null))
            _surroundingTiles.Add(tile);

        return base.OnSelected(ownTile);
    }

    public override DeselectStatus OnClicked(GameObject ownTile, GameObject clickedTile) {
        if (!_isBuilding) {
            ResetModifiedTiles(_surroundingTiles.ToArray());
            _surroundingTiles.Clear();
            return base.OnClicked(ownTile, clickedTile);
        }
        _isBuilding = false;
        ResetModifiedTiles(_surroundingTiles.ToArray());
        ownTile.GetComponent<TileController>().ResetSprite();
        if(_hoveredTile != null) _hoveredTile.ResetSprite();

        TileController tileTwo = clickedTile.GetComponent<TileController>();
        if (!_surroundingTiles.Contains(tileTwo)) {
            _surroundingTiles.Clear();
            return DeselectStatus.Both;
        }
        _surroundingTiles.Clear();

        GameObject structure = (GameObject) Instantiate(_buildType, clickedTile.transform.position, Quaternion.identity);
		if (!tileTwo.IsTraversable (structure)) {
			GameObject.Destroy (structure);
			return DeselectStatus.Both;
		}
        BaseUnit structBase = structure.GetComponent<BaseUnit>();
        structBase.Owner = GetComponent<BaseUnit>().Owner;
		structBase.Owner.MoneyAmount -= structBase.GetCost (ownTile.GetComponent<TileController>().Environment);
		structBase.Owner.Moves -= 1;
		structBase.GetComponent<SpriteRenderer> ().sprite = structBase.Owner.BarrackSprite;

        _buildType = null;
        if (tileTwo.Unit != null) {
            if (tileTwo.IsTraversable(structure))
                tileTwo.Unit.StackSize++;
            GameObject.Destroy(structure);
            return DeselectStatus.Both;
        }
        else
            tileTwo.Unit = structBase;
        return DeselectStatus.Both;
    }

    public override void OnMouseEnter(GameObject ownTile, GameObject hoveredTile) {
        if (!_isBuilding || ownTile == hoveredTile) {
            base.OnMouseEnter(ownTile, hoveredTile);
            return;
        }

        TileController hoverTile = hoveredTile.GetComponent<TileController>();
        if (!_surroundingTiles.Contains(hoverTile)) {
            _hoveredTile = hoverTile;
            _hoveredTile.GetComponent<SpriteRenderer>().color = BuildNotAllowedColor;
        }
    }

    public override void OnMouseLeave(GameObject ownTile, GameObject hoveredTile) {
        if (!_isBuilding) {
            base.OnMouseLeave(ownTile, hoveredTile);
            return;
        }
        if(_hoveredTile != null && !_surroundingTiles.Contains(_hoveredTile))
            _hoveredTile.ResetSprite();
        _hoveredTile = null;
    }

    public void CreateStructure(string structureName) {
        _isBuilding = true;
        _buildType = GetComponent<SoldierUnit>().BuildableStructures.Single(x => x.name == structureName);

        GameObject mockStructure = GameObject.Instantiate(_buildType);
        mockStructure.GetComponent<BaseUnit>().Owner = GetComponent<BaseUnit>().Owner;
        foreach(TileController tile in _surroundingTiles)
            if (tile.IsTraversable(mockStructure))
                tile.GetComponent<SpriteRenderer>().color = BuildAllowedColor;
            else
                tile.GetComponent<SpriteRenderer>().color = BuildNotAllowedColor;
        GameObject.Destroy(mockStructure);
    }

}