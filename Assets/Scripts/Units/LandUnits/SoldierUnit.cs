using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SoldierUnit : LandUnit {

    public GameObject[] BuildableStructures;

    public Color BuildAllowedColor;
    public Color BuildNotAllowedColor;

    private bool _isBuilding;
    private GameObject _buildType;

    private List<TileController> _surroundingTiles;
    private TileController _hoveredTile;


    public override DeselectStatus OnFirstSelected(GameObject firstTile) {
        ActionBarController actionBar = GameObject.Find("ActionBar").GetComponent<ActionBarController>();
        foreach(GameObject unit in BuildableStructures)
            actionBar.AddButton(unit.name, CreateStructure);

        TileController thisTile = firstTile.GetComponent<TileController>();

        TileController[] directions = { thisTile.Left, thisTile.Up, thisTile.Right, thisTile.Down };
        _surroundingTiles = new List<TileController>();
        foreach (TileController tile in directions.Where(x => x != null))
            _surroundingTiles.Add(tile);

        return base.OnFirstSelected(firstTile);
    }

    public override DeselectStatus OnSecondClicked(GameObject firstTile, GameObject secondTile) {
        if(!_isBuilding)
            return base.OnSecondClicked(firstTile, secondTile);

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

    public void CreateStructure(string structureName) {
        _isBuilding = true;
        _buildType = BuildableStructures.Single(x => x.name == structureName);
        GameObject unitMock = (GameObject)Instantiate(_buildType, Vector3.zero, Quaternion.identity);
        unitMock.GetComponent<BaseUnit>().Owner = this.Owner;

        foreach (TileController tile in _surroundingTiles)
            if (tile.IsTraversable(unitMock))
                tile.GetComponent<SpriteRenderer>().color = BuildAllowedColor;
            else
                tile.GetComponent<SpriteRenderer>().color = BuildNotAllowedColor;
        GameObject.Destroy(unitMock);
    }

    public override void OnMouseEnter(GameObject firstTile, GameObject secondTile) {
        if (!_isBuilding || firstTile == secondTile)
            base.OnMouseEnter(firstTile, secondTile);

        TileController hoverTile = secondTile.GetComponent<TileController>();
        if (!_surroundingTiles.Contains(hoverTile)) {
            _hoveredTile = hoverTile;
            _hoveredTile.GetComponent<SpriteRenderer>().color = BuildNotAllowedColor;
        }
    }

    public override void OnMouseLeave(GameObject firstTile, GameObject secondTile) {
        base.OnMouseLeave(firstTile, secondTile);
        if (_hoveredTile != null && !_surroundingTiles.Contains(_hoveredTile))
            _hoveredTile.ResetSprite();
        _hoveredTile = null;
    }
}
