using UnityEngine;
using System.Linq;

public class BaseStructure : StructureUnit {

    public GameObject[] BuildableUnits;


    private bool _isBuilding;
    private GameObject _buildType;

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
}
