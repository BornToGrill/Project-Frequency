using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterUnitEventController : LandUnitEventController {


	public override void OnMouseEnter(GameObject ownTile, GameObject hoveredTile) {
		base.OnMouseEnter (ownTile, hoveredTile);
	    if (IsSplitting)
	        return;

		GameObject carryUnit = GetComponent<WaterUnit>().CarryUnit;
		TileController tileOne = ownTile.GetComponent<TileController>();
		TileController tileTwo = hoveredTile.GetComponent<TileController>();
		List<TileController> path = Pathfinding.FindPath (tileOne, tileTwo).Path;
		int index = -1;

		if (carryUnit != null) {
			index = path.FindIndex(x => x.IsTraversable(carryUnit));
		}
		if (!(index < 0))
			if (!path.Last ().IsTraversable (carryUnit))
				foreach (var element in path)
					element.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
	}

    #region Movement
    public override DeselectStatus MoveToEmpty(TileController start, List<TileController> path) {

        MoveWaterUnit(start, path, null);

        return DeselectStatus.Both;
    }
    public override DeselectStatus MoveToAttack(TileController start, List<TileController> path) {
        LandUnit carry = ((WaterUnit) start.Unit).CarryUnit.GetComponent<LandUnit>();
        List<TileController> movePath = path.Take(path.Count - carry.Range).ToList();
        MoveWaterUnit(start, movePath,
            () => {
                carry.Attack(path.Last().Unit);
            });

        return DeselectStatus.Both;
    }

    public override DeselectStatus MoveToMerge(TileController start, List<TileController> path) {
        LandUnit carry = ((WaterUnit) start.Unit).CarryUnit.GetComponent<LandUnit>();
        LandUnit mergeTarget = (LandUnit) path.Last().Unit;
        MoveWaterUnit(start, path, () => { mergeTarget.Merge(carry); });

        path.Last().Unit = mergeTarget;

        return DeselectStatus.Both;
    }

    private void MoveWaterUnit(TileController startTile, List<TileController> path, Action finalAction) {
        GameObject carryUnit = GetComponent<WaterUnit>().CarryUnit;
        WaterUnit startBoat = (WaterUnit) startTile.Unit;
        BaseUnit unit = null;
        int index = -1;

        if (carryUnit != null) {
            unit = carryUnit.GetComponent<BaseUnit>();
            index = path.FindIndex(x => x.IsTraversable(carryUnit));
        }

        if (index < 0) {
            StartCoroutine(AnimateToTile(path));
            path.Last().Unit = GetComponent<BaseUnit>();
        }
        else {
			if (!path.Last ().IsTraversable (carryUnit)) {
				carryUnit.GetComponent<BaseUnit> ().Owner.Moves += path.Count;
				return;
			}
            TileController[] boatPath = path.Take(index).ToArray();
            TileController[] unitPath = path.Skip(index).ToArray();
			if (boatPath.Length > 0) {
				StartCoroutine (AnimateToTile (boatPath, () => {
					startBoat.UnloadUnit (boatPath.Last ().gameObject);
					unit.StartCoroutine (unit.GetComponent<LandUnitEventController> ().AnimateToTile (unitPath, finalAction));
				}));
				boatPath.Last ().Unit = GetComponent<BaseUnit> ();
				unitPath.Last ().Unit = unit;
			} else {
				startBoat.UnloadUnit (unitPath.First ().gameObject);
				unit.StartCoroutine(unit.GetComponent<LandUnitEventController>().AnimateToTile(unitPath, finalAction));
				unitPath.Last ().Unit = unit;
			}
        }

		if(index != 0) // If boat also moves.
        	startTile.Unit = null;
    }

    #endregion

    #region Unit Spliting

    public override void ShowUnitStack(TileController ownTile) {
        GameObject carry = GetComponent<WaterUnit>().CarryUnit;
        if (carry == null)
            return;
        GameObject stackOverlay = GameObject.Find("UnitStack");
        StackWindow window = stackOverlay.GetComponent<StackWindow>();
        BaseUnit unit = carry.GetComponent<BaseUnit>();
        window.Show(unit.GetComponent<LandUnitEventController>().StackSizeSprite, unit.Owner.Color, unit.StackSize, unit.CurrentPlayerPredicate(ownTile), UnitSplitCallback);
    }

    protected override GameObject CreateSplitMock() {
        GameObject mock = Instantiate(GetComponent<WaterUnit>().CarryUnit);
        mock.SetActive(true);
        mock.name = GetComponent<WaterUnit>().CarryUnit.name;
        BaseUnit unit = mock.GetComponent<BaseUnit>();
        unit.StackSize = SplitAmount;
        unit.Owner = GetComponent<BaseUnit>().Owner;
        return mock;
    }

    public override DeselectStatus Split(TileController ownTile, TileController targetTile) {
        IsSplitting = false;
        if (!_surrTiles.Contains(targetTile)) {
            ResetSplitTiles();
            return DeselectStatus.Both;
        }
        GameObject mock = CreateSplitMock();
        BaseUnit unit = mock.GetComponent<BaseUnit>();
        if (unit.Owner.Moves < 1) {
            Destroy(mock);
            ResetSplitTiles();
            return DeselectStatus.Both;
        }
        WaterUnit water = GetComponent<WaterUnit>();
        if (!targetTile.IsTraversable(mock)) {
            Destroy(mock);
            ResetSplitTiles();
            return DeselectStatus.Both;
        }
        if (water.CarryUnit.GetComponent<BaseUnit>().StackSize <= unit.StackSize) {
            Destroy(water.CarryUnit);
            water.CarryUnit = null;
        }
        else
            water.CarryUnit.GetComponent<BaseUnit>().StackSize -= SplitAmount;

        LandUnitEventController landUnit = mock.GetComponent<LandUnitEventController>();
        mock.SetActive(true);
        if (targetTile.Unit == null)
            landUnit.MoveToEmpty(null, new List<TileController>() { targetTile });
        else
            landUnit.MoveToMerge(null, new List<TileController>() { targetTile });
        unit.Owner.Moves--;
        ResetSplitTiles();
        return DeselectStatus.Both;
    }

    #endregion

}
