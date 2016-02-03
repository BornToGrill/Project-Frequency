using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterUnitEventController : LandUnitEventController {


    public override DeselectStatus OnClicked(GameObject ownTile, GameObject clickedTile) {
        if (IsSplitting)
            return base.OnClicked(ownTile, clickedTile);

        ResetModifiedTiles();
        if (ownTile == clickedTile)
            return DeselectStatus.Both;
        if(!GetComponent<BaseUnit>().CurrentPlayerPredicate(ownTile.GetComponent<TileController>()))
            return DeselectStatus.Both;
        WaterUnit boat = GetComponent<WaterUnit>();
        GameObject carryUnit = boat.CarryUnit;
        TileController tileOne = ownTile.GetComponent<TileController>();
        TileController tileTwo = clickedTile.GetComponent<TileController>();
        StateController multiplayerController = GameObject.Find("Board").GetComponent<StateController>();

        if (tileTwo.Unit != null && tileTwo.Unit is WaterUnit)
            return DeselectStatus.Both;
        if (carryUnit == null) {
            PathFindingResult boatPath = Pathfinding.FindPath(tileOne, tileTwo);
            if (boatPath.FoundEndPoint && boatPath.ValidPath && boatPath.Path.Count <= boat.Owner.Moves &&
                boatPath.Path.Last().IsTraversable(gameObject)) {
                boat.Owner.Moves -= boatPath.Path.Count;
                if (multiplayerController != null)
                    multiplayerController.ServerComs.Notify.Move(MoveType.Empty, tileOne, tileTwo);
                MoveToEmpty(tileOne, boatPath.Path);
            }
            return DeselectStatus.Both;
        }

        PathFindingResult path = Pathfinding.FindPath(tileOne, tileTwo);

        if (!carryUnit.GetComponent<BaseUnit>().TraversableEnvironments.Contains(path.Path.Last().Environment)) {
            boat.TraversableEnvironments = boat._defaultEnvironments;
            path = Pathfinding.FindPath(tileOne, tileTwo);
            boat.TraversableEnvironments =
                boat._defaultEnvironments.Concat(carryUnit.GetComponent<BaseUnit>().TraversableEnvironments).ToArray();
            if (!path.FoundEndPoint || !path.ValidPath || boat.Owner.Moves < path.Path.Count)
                return DeselectStatus.Both;
            else {
                boat.Owner.Moves -= path.Path.Count;
                if (multiplayerController != null)
                    multiplayerController.ServerComs.Notify.Move(MoveType.Empty, tileOne, tileTwo);
                MoveToEmpty(tileOne, path.Path);
            }
        }
        else {
            if (tileTwo.Unit != null) {
                if (tileTwo.IsTraversable(carryUnit) && boat.Owner.Moves >= path.Path.Count) {
                    boat.Owner.Moves -= path.Path.Count;
                    if (multiplayerController != null)
                        multiplayerController.ServerComs.Notify.Move(MoveType.Merge, tileOne, tileTwo);
                    MoveToMerge(tileOne, path.Path);
                }
                else if (tileTwo.Unit.Owner != boat.Owner && boat.Owner.Moves >= path.Path.Count) {
                    if (path.Path.Count - carryUnit.GetComponent<LandUnit>().Range < 0) {
                        if (GetComponent<BaseUnit>().Owner.Moves < 1)
                            return DeselectStatus.Both;
                        boat.Owner.Moves -= 1;
                    }
                    else
                        boat.Owner.Moves -= path.Path.Count - carryUnit.GetComponent<LandUnit>().Range + 1;

                    if (multiplayerController != null)
                        multiplayerController.ServerComs.Notify.Move(MoveType.Attack, tileOne, tileTwo);
                    MoveToAttack(tileOne, path.Path);
                }
            }
            else {
                if (boat.Owner.Moves >= path.Path.Count) {
                    boat.Owner.Moves -= path.Path.Count;
                    if (multiplayerController != null)
                        multiplayerController.ServerComs.Notify.Move(MoveType.Empty, tileOne, tileTwo);
                    MoveToEmpty(tileOne, path.Path);
                }
            }
        }

        return DeselectStatus.Both;
    }

    public override void OnMouseEnter(GameObject ownTile, GameObject hoveredTile) {
	    if (ownTile == hoveredTile)
	        return;
	    if (IsSplitting)
	        return;
	    if (!GetComponent<BaseUnit>().CurrentPlayerPredicate(ownTile.GetComponent<TileController>()))
	        return;

        WaterUnit boat = GetComponent<WaterUnit>();
        GameObject carryUnit = boat.CarryUnit;
		TileController tileOne = ownTile.GetComponent<TileController>();
	    TileController tileTwo = hoveredTile.GetComponent<TileController>();

        if (carryUnit == null) { // Only boat moves
            PathFindingResult boatPath = Pathfinding.FindPath(tileOne, tileTwo);
            ModifiedTiles = boatPath.Path;
            if (tileTwo.Unit != null && tileTwo.Unit is WaterUnit)
                foreach (TileController tile in ModifiedTiles)
                    tile.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
                else if (!boatPath.FoundEndPoint || !boatPath.ValidPath || boatPath.Path.Count > boat.Owner.Moves ||
                !boatPath.Path.Last().IsTraversable(gameObject))
                foreach (TileController tile in ModifiedTiles)
                    tile.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
            else {
                foreach (TileController tile in ModifiedTiles)
                    tile.GetComponent<SpriteRenderer>().color = MoveColor;
            }
                return;
        }

        //TODO: TEST
        PathFindingResult path = Pathfinding.FindPath(tileOne, tileTwo);
	    ModifiedTiles = path.Path;
        if (tileTwo.Unit != null && tileTwo.Unit is WaterUnit) {
            foreach (TileController tile in ModifiedTiles)
                tile.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
        }
        else if (!carryUnit.GetComponent<BaseUnit>().TraversableEnvironments.Contains(path.Path.Last().Environment)) {
	        // Only boat moves
	        boat.TraversableEnvironments = boat._defaultEnvironments;
	        path = Pathfinding.FindPath(tileOne, tileTwo);
	        ResetModifiedTiles();
	        ModifiedTiles = path.Path;
	        if (!path.FoundEndPoint || !path.ValidPath || boat.Owner.Moves < path.Path.Count) {
	            foreach (TileController tile in ModifiedTiles)
	                tile.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
	        }
	        else
	            foreach (TileController tile in ModifiedTiles)
	                tile.GetComponent<SpriteRenderer>().color = MoveColor;
	    }
	    else {
	        if (tileTwo.Unit != null) {
	            if (tileTwo.IsTraversable(carryUnit) && boat.Owner.Moves >= path.Path.Count) {
	                // Merge
	                foreach (TileController tile in ModifiedTiles)
	                    tile.GetComponent<SpriteRenderer>().color = MoveColor;
	            }
	            else if (tileTwo.Unit.Owner != boat.Owner && boat.Owner.Moves >= path.Path.Count) {
	                if (path.Path.Count <= boat.CarryUnit.GetComponent<LandUnit>().Range && boat.Owner.Moves > 0)
	                    ModifiedTiles.Last().GetComponent<SpriteRenderer>().color = AttackColor; // TODO: Tank checking ( range )
	                else if (path.Path[path.Path.Count - 1 - boat.CarryUnit.GetComponent<LandUnit>().Range].IsTraversable(carryUnit)) { // TODO: Tank range
	                    // Attack on land
	                    for (int i = 0; i < ModifiedTiles.Count - GetComponent<LandUnit>().Range; i++)
	                        ModifiedTiles[i].GetComponent<SpriteRenderer>().color = MoveColor;
	                    ModifiedTiles.Last().GetComponent<SpriteRenderer>().color = AttackColor;
	                }
	                else {
	                    for (int i = 0; i < ModifiedTiles.Count - carryUnit.GetComponent<LandUnit>().Range; i++)
	                        ModifiedTiles[i].GetComponent<SpriteRenderer>().color = MoveColor;
	                    ModifiedTiles.Last().GetComponent<SpriteRenderer>().color = AttackColor;
	                }
	            }
	            else {
	                foreach (TileController tile in ModifiedTiles)
	                    tile.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
	            }
	        }
	        else {
	            // Move to empty
	            if (boat.Owner.Moves >= path.Path.Count)
	                foreach (TileController tile in ModifiedTiles)
	                    tile.GetComponent<SpriteRenderer>().color = MoveColor;
	            else
	                foreach (TileController tile in ModifiedTiles)
	                    tile.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
	        }
	    }



	    boat.TraversableEnvironments =
	        boat._defaultEnvironments.Concat(carryUnit.GetComponent<BaseUnit>().TraversableEnvironments).ToArray();

        //TODO: Test
	}

    #region Movement
    public override DeselectStatus MoveToEmpty(TileController start, List<TileController> path) {

        MoveWaterUnit(start, path, QueueNextItem);

        return DeselectStatus.Both;
    }
    public override DeselectStatus MoveToAttack(TileController start, List<TileController> path) {
        LandUnit carry = ((WaterUnit) start.Unit).CarryUnit.GetComponent<LandUnit>();
        List<TileController> movePath = path.Take(path.Count - carry.Range).ToList();
        MoveWaterUnit(start, movePath,
            () => {
                path.Last().Unit.DamageUnit(carry._stackDamage, GetComponent<BaseUnit>());
                //carry.Attack(path.Last().Unit);
                QueueNextItem();
            });

        return DeselectStatus.Both;
    }

    public override DeselectStatus MoveToMerge(TileController start, List<TileController> path) {
        LandUnit carry = ((WaterUnit) start.Unit).CarryUnit.GetComponent<LandUnit>();
        LandUnit mergeTarget = (LandUnit) path.Last().Unit;
        MoveWaterUnit(start, path, () => {
            mergeTarget.Merge(carry);
            QueueNextItem();
        });

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
            StartCoroutine(AnimateToTile(path, finalAction));
            if(path.Count > 0)
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

		if(index != 0 && path.Count > 0) // If boat also moves.
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

    public override GameObject CreateSplitMock() {
        GameObject mock = Instantiate(GetComponent<WaterUnit>().CarryUnit);
        mock.SetActive(true);
        mock.transform.position = transform.position;
        mock.name = GetComponent<WaterUnit>().CarryUnit.name;
        BaseUnit unit = mock.GetComponent<BaseUnit>();
        unit.StackSize = SplitAmount;
        unit.Owner = GetComponent<BaseUnit>().Owner;
        return mock;
    }

    public override DeselectStatus Split(GameObject mock, TileController ownTile, TileController targetTile) {
        IsSplitting = false;

        BaseUnit unit = mock.GetComponent<BaseUnit>();

        WaterUnit water = GetComponent<WaterUnit>();

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

    private void QueueNextItem() {
        GameObject.Find("Board").GetComponent<GameController>().NextQueueItem = true;
    }

}
