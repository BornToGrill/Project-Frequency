using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterUnitEventController : LandUnitEventController {



    #region Movement
    public override DeselectStatus MoveToEmpty(TileController start, List<TileController> path) {


        GameObject carryUnit = GetComponent<WaterUnit>().CarryUnit;
        WaterUnit startBoat = (WaterUnit)start.Unit;
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
            TileController[] boatPath = path.Take(index).ToArray();
            TileController[] unitPath = path.Skip(index).ToArray();

            var x = ((WaterUnit) start.Unit);
            var y = unit.GetComponent<LandUnitEventController>();
            StartCoroutine(AnimateToTile(boatPath, () => {
                startBoat.UnloadUnit(boatPath.Last().gameObject);
                unit.StartCoroutine(unit.GetComponent<LandUnitEventController>().AnimateToTile(unitPath));
            }));
            boatPath.Last().Unit = GetComponent<BaseUnit>();
            unitPath.Last().Unit = unit;
        }
        start.Unit = null;


        return DeselectStatus.Both;
    }
    public override DeselectStatus MoveToAttack(TileController start, List<TileController> path) {
        List<TileController> movePath = path.Take(path.Count - GetComponent<LandUnit>().Range).ToList();
        if (movePath.Count > 0) {
            start.Unit = null;
            movePath.Last().Unit = GetComponent<BaseUnit>();
        }

        StartCoroutine(AnimateToTile(movePath, () => {
            GetComponent<LandUnit>().Attack(path.Last().Unit);
        }));

        return DeselectStatus.Both;
    }

    public override DeselectStatus MoveToMerge(TileController start, List<TileController> path) {
        start.Unit = null;

        StartCoroutine(AnimateToTile(path, () => {
            ((LandUnit) path.Last().Unit).Merge(GetComponent<BaseUnit>());
        }));

        return DeselectStatus.Both;
    }
    #endregion

}
