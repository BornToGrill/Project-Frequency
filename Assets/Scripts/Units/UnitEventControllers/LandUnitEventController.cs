﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LandUnitEventController : EventControllerBase {

    public Color MoveColor;
    public Color InvalidMoveColor;
    public Color AttackColor;

    public Sprite StackSizeSprite;

    public float MovementSpeed;

    private TileController[] _surrTiles;
    private bool _isSplitting;
    private int _splitAmount;

    public override DeselectStatus OnSelected(GameObject ownTile) {
        ownTile.GetComponent<SpriteRenderer>().color = SelfSelectedColor;
		UnitStats unitStats = GameObject.Find("UnitStats").GetComponent<UnitStats>();
		if (GetComponent<LandUnit>()._stackDamage == 0)
			unitStats.Set (GetComponent<LandUnit> ()._stackDamage, GetComponent<LandUnit> ().Health);
		else
			unitStats.Set (GetComponent<LandUnit> ()._stackDamage, GetComponent<LandUnit> ()._stackHealth);

        ShowUnitStack(ownTile.GetComponent<TileController>());

        TileController own = ownTile.GetComponent<TileController>();
        _surrTiles = new[] { own.Left, own.Up, own.Right, own.Down }.Where(x => x != null).ToArray();

        return DeselectStatus.None;
    }

    public override DeselectStatus OnClicked(GameObject ownTile, GameObject clickedTile) {
        if (ownTile == clickedTile) {
            ResetModifiedTiles(ownTile.GetComponent<TileController>());
            return DeselectStatus.Both;
        }

        if (_isSplitting)
            return Split(ownTile.GetComponent<TileController>(), clickedTile.GetComponent<TileController>());

        StateController multiplayerController = GameObject.Find("Board").GetComponent<StateController>();

        ResetModifiedTiles(ownTile.GetComponent<TileController>());
        TileController tileOne = ownTile.GetComponent<TileController>();
        TileController tileTwo = clickedTile.GetComponent<TileController>();
        if (!GetComponent<BaseUnit>().CurrentPlayerPredicate(tileOne))
            return DeselectStatus.Both;

        PathFindingResult path = Pathfinding.FindPath(tileOne, tileTwo);
		Player owner = GetComponent<BaseUnit> ().Owner;

		if (path.FoundEndPoint == false || !path.ValidPath)
			return DeselectStatus.Both;

		if (tileTwo.Unit == null) {
			if (!tileTwo.IsTraversable(gameObject))
				return DeselectStatus.Both;
			if (path.Path.Count > owner.Moves)
				return DeselectStatus.Both;
			else {
				owner.Moves -= path.Path.Count;
			    if (multiplayerController != null)
			        multiplayerController.ServerComs.Notify.Move(MoveType.Empty, tileOne, tileTwo);
				return MoveToEmpty (tileOne, path.Path);
			}
		}
        else {
			if (tileTwo.Unit.Owner != GetComponent<BaseUnit> ().Owner) {
				if (path.Path.Count - GetComponent<LandUnit> ().Range + 1 > owner.Moves)
					return DeselectStatus.Both;
				else {
					owner.Moves -= (path.Path.Count - GetComponent<LandUnit> ().Range + 1);
				    if (multiplayerController != null)
				        multiplayerController.ServerComs.Notify.Move(MoveType.Attack, tileOne, tileTwo);
					return MoveToAttack (tileOne, path.Path);
				}
			}
            else {
                if (tileTwo.IsTraversable(gameObject))
                    if (path.Path.Count <= owner.Moves) {
                        owner.Moves -= path.Path.Count;
                        if (multiplayerController != null)
                            multiplayerController.ServerComs.Notify.Move(MoveType.Merge, tileOne, tileTwo);
                        return MoveToMerge(tileOne, path.Path);
                    }
			    return DeselectStatus.Both;
            }
        }
    }

    public override void OnMouseEnter(GameObject ownTile, GameObject hoveredTile) {
        if (ownTile == hoveredTile)
            return;
        
        TileController tileOne = ownTile.GetComponent<TileController>();
        TileController tileTwo = hoveredTile.GetComponent<TileController>();
        if (!GetComponent<BaseUnit>().CurrentPlayerPredicate(tileOne))
            return;
        if (_isSplitting) {
            GameObject temp = CreateSplitMock();
            if (hoveredTile.GetComponent<TileController>().IsTraversable(temp))
                hoveredTile.GetComponent<SpriteRenderer>().color = SelfSelectedColor;
            else
                hoveredTile.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
            GameObject.Destroy(temp);
            return;
        }
        PathFindingResult path = Pathfinding.FindPath(tileOne, tileTwo);

        ModifiedTiles = path.Path;

        if (path.FoundEndPoint == false || !path.ValidPath)
            foreach (var element in ModifiedTiles)
                element.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
        else {
            if (tileTwo.Unit == null) {
				if (tileTwo.IsTraversable (gameObject)) {
					Color result = path.Path.Count > GetComponent<BaseUnit> ().Owner.Moves ? InvalidMoveColor : MoveColor;
					foreach (var element in ModifiedTiles)
						element.GetComponent<SpriteRenderer> ().color = result;
				}
                else
                    foreach (var element in ModifiedTiles)
                        element.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
            }
            else {
				if (tileTwo.Unit.Owner != GetComponent<BaseUnit> ().Owner) {
					if (path.Path.Count - GetComponent<LandUnit> ().Range + 1 > GetComponent<BaseUnit> ().Owner.Moves)
						foreach (var element in ModifiedTiles)
							element.GetComponent<SpriteRenderer> ().color = InvalidMoveColor;
					else {
						for (int i = 0; i < ModifiedTiles.Count - GetComponent<LandUnit> ().Range; i++)
							ModifiedTiles [i].GetComponent<SpriteRenderer> ().color = MoveColor;
						ModifiedTiles.Last ().GetComponent<SpriteRenderer> ().color = AttackColor;
					}
				} else if (tileTwo.IsTraversable (gameObject)) {
					Color result = path.Path.Count > GetComponent<BaseUnit> ().Owner.Moves ? InvalidMoveColor : MoveColor;
					foreach (var element in ModifiedTiles)
						element.GetComponent<SpriteRenderer> ().color = result;
				}
                else
                    foreach (var element in ModifiedTiles)
                        element.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
            }
        }
    }

    public override void OnMouseLeave(GameObject ownTile, GameObject hoveredTile) {
        ResetModifiedTiles();
        if (ownTile == hoveredTile)
            return;
        if (_isSplitting) {
            GameObject temp = CreateSplitMock();
            if (_surrTiles.Contains(hoveredTile.GetComponent<TileController>())) {
                if (hoveredTile.GetComponent<TileController>().IsTraversable(temp))
                    hoveredTile.GetComponent<SpriteRenderer>().color = MoveColor;
                else
                    hoveredTile.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
            }
            else
                hoveredTile.GetComponent<TileController>().ResetSprite();
            Destroy(temp);
        }
    }

    protected void ResetModifiedTiles(params TileController[] additionalTiles) {
        foreach (TileController tile in ModifiedTiles)
            tile.ResetSprite();
        foreach (TileController tile in additionalTiles)
            tile.ResetSprite();
        ModifiedTiles.Clear();
    }

    #region Movement
    public virtual DeselectStatus MoveToEmpty(TileController start, List<TileController> path) {
        StartCoroutine(AnimateToTile(path));
        if(start != null)
            start.Unit = null;
        path.Last().Unit = GetComponent<BaseUnit>();
        return DeselectStatus.Both;
    }

    public virtual DeselectStatus MoveToAttack(TileController start, List<TileController> path) {
        List<TileController> movePath = path.Take(path.Count - GetComponent<LandUnit>().Range).ToList();
        if (movePath.Count > 0) {
            if(start != null)
                start.Unit = null;
            movePath.Last().Unit = GetComponent<BaseUnit>();
        }

        StartCoroutine(AnimateToTile(movePath, () => {
            GetComponent<LandUnit>().Attack(path.Last().Unit);
        }));

        return DeselectStatus.Both;
    }

    public virtual DeselectStatus MoveToMerge(TileController start, List<TileController> path) {
        if(start != null)
            start.Unit = null;
        LandUnit mergeTarget = (LandUnit) path.Last().Unit;

        StartCoroutine(AnimateToTile(path, () => {
            mergeTarget.Merge(GetComponent<BaseUnit>());
        }));

        return DeselectStatus.Both;
    }
    #endregion

    #region Unit Splitting

    public virtual void ShowUnitStack(TileController ownTile) {
        GameObject stackOverlay = GameObject.Find("UnitStack");
        StackWindow window = stackOverlay.GetComponent<StackWindow>();
        BaseUnit unit = GetComponent<BaseUnit>();
        window.Show(StackSizeSprite, unit.Owner.Color, unit.StackSize, unit.CurrentPlayerPredicate(ownTile), UnitSplitCallback);
    }

    public virtual void UnitSplitCallback(int amount) {
        _isSplitting = true;
        _splitAmount = amount;
        GameObject mock = CreateSplitMock();
        foreach (TileController tile in _surrTiles) {
            if (tile.IsTraversable(mock))
                tile.GetComponent<SpriteRenderer>().color = MoveColor;
            else
                tile.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
        }
        Destroy(mock);
    }

    public virtual DeselectStatus Split(TileController ownTile, TileController targetTile) {
        _isSplitting = false;
        GameObject mock = CreateSplitMock();
        BaseUnit unit = mock.GetComponent<BaseUnit>();
        if (!targetTile.IsTraversable(mock)) {
            Destroy(mock);
            return DeselectStatus.Both;
        }
        if (GetComponent<BaseUnit>().StackSize <= unit.StackSize) {
            ownTile.Unit = null;
            Destroy(gameObject); //TODO: Test if this works.
        }
        else
            ownTile.Unit.StackSize -= _splitAmount;
        LandUnitEventController landUnit = mock.GetComponent<LandUnitEventController>();
        if (targetTile.Unit == null)
            landUnit.MoveToEmpty(null, new List<TileController>() { targetTile });
        else
            landUnit.MoveToMerge(null, new List<TileController>() { targetTile });
        return DeselectStatus.Both;
    }

    private GameObject CreateSplitMock() {
        GameObject mock = Instantiate(gameObject);
        mock.name = gameObject.name;
        BaseUnit unit = mock.GetComponent<BaseUnit>();
        unit.StackSize = _splitAmount;
        unit.Owner = GetComponent<BaseUnit>().Owner;
        return mock;
    }
    #endregion

    internal IEnumerator AnimateToTile(IEnumerable<TileController> path) {
        yield return AnimateToTile(path, null);
    }
    internal IEnumerator AnimateToTile(IEnumerable<TileController> path, Action endAction) {
		
        foreach (TileController tile in path) {
			StartSpriteAnimation (tile.transform.position, transform.position);
            Vector3 startPosition = transform.position;
            for (float i = 0.1f; i <= 1f * MovementSpeed; i += 0.1f) {
                transform.position = Vector3.Lerp(startPosition, tile.transform.position, i);
                yield return null;
            }
			StartSpriteAnimation (tile.transform.position, transform.position);
            transform.position = tile.transform.position;
        }
        if (endAction != null)
            endAction.Invoke();
    }

	private void StartSpriteAnimation(Vector3 direction, Vector3 position) {
		Animator anim = GetComponent<Animator> ();
		BaseUnit unit = GetComponent<BaseUnit> ();

		if (direction.x - position.x < 0)
			anim.SetInteger ("Direction", 3); // Left
		else if (direction.x - position.x > 0)
			anim.SetInteger ("Direction", 1); // Right
		else if (direction.y - position.y > 0)
			anim.SetInteger ("Direction", 0); // Up
		else if (direction.y - position.y < 0)
			anim.SetInteger ("Direction", 2); // Down
	}
}
