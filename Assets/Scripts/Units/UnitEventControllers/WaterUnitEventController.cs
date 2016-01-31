using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterUnitEventController : LandUnitEventController {


	public override void OnMouseEnter(GameObject ownTile, GameObject hoveredTile) {
		base.OnMouseEnter (ownTile, hoveredTile);
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

}
