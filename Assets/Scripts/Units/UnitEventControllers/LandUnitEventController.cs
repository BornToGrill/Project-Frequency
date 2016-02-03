using System;
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

    protected TileController[] _surrTiles;
    protected bool IsSplitting;
    public int SplitAmount;

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

        ResetModifiedTiles(ownTile.GetComponent<TileController>());
        TileController tileOne = ownTile.GetComponent<TileController>();
        TileController tileTwo = clickedTile.GetComponent<TileController>();
        StateController multiplayerController = GameObject.Find("Board").GetComponent<StateController>();

        if (IsSplitting) {
            GameObject mock = CreateSplitMock();
            if (mock.GetComponent<BaseUnit>().Owner.Moves < 1 ||
                !clickedTile.GetComponent<TileController>().IsTraversable(mock) ||
                !_surrTiles.Contains(clickedTile.GetComponent<TileController>())) {
                ResetSplitTiles();
                Destroy(mock);
                return DeselectStatus.Both;
            }
            if (multiplayerController != null)
                multiplayerController.ServerComs.Notify.SplitUnit(tileOne, tileTwo, SplitAmount);

            return Split(mock, tileOne, tileTwo);
        }



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
				    if (path.Path.Count - GetComponent<LandUnit>().Range < 0) {
				        if (GetComponent<BaseUnit>().Owner.Moves < 1)
				            return DeselectStatus.Both;
				        owner.Moves -= 1;
				    }
				    else
				        owner.Moves -= path.Path.Count - GetComponent<LandUnit>().Range + 1;
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
        if (IsSplitting) {
            GameObject temp = CreateSplitMock();
            if (_surrTiles.Contains(tileTwo) && temp.GetComponent<BaseUnit>().Owner.Moves > 0 && tileTwo.IsTraversable(temp))
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
					if (path.Path.Count - GetComponent<LandUnit> ().Range + 1 > GetComponent<BaseUnit> ().Owner.Moves || (path.Path.Count - GetComponent<LandUnit>().Range < 0 && GetComponent<BaseUnit>().Owner.Moves < 1))
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
        if (IsSplitting) {
            GameObject temp = CreateSplitMock();
            if (_surrTiles.Contains(hoveredTile.GetComponent<TileController>())) {
                if (temp.GetComponent<BaseUnit>().Owner.Moves > 0 && hoveredTile.GetComponent<TileController>().IsTraversable(temp))
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
        StartCoroutine(AnimateToTile(path, QueueNextItem));
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
            QueueNextItem();
        }));

        return DeselectStatus.Both;
    }

    public virtual DeselectStatus MoveToMerge(TileController start, List<TileController> path) {
        if(start != null)
            start.Unit = null;
        LandUnit mergeTarget = (LandUnit) path.Last().Unit;

        StartCoroutine(AnimateToTile(path, () => {
            mergeTarget.Merge(GetComponent<BaseUnit>());
            QueueNextItem();
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
        IsSplitting = true;
        SplitAmount = amount;
        GameObject mock = CreateSplitMock();
        foreach (TileController tile in _surrTiles) {
            if (mock.GetComponent<BaseUnit>().Owner.Moves > 0 && tile.IsTraversable(mock))
                tile.GetComponent<SpriteRenderer>().color = MoveColor;
            else
                tile.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
        }
        Destroy(mock);
    }

    public virtual DeselectStatus Split(GameObject mock, TileController ownTile, TileController targetTile) {
        IsSplitting = false;
        BaseUnit unit = mock.GetComponent<BaseUnit>();

        if (GetComponent<BaseUnit>().StackSize <= unit.StackSize) {
            ownTile.Unit = null;
            Destroy(gameObject);
        }
        else
            ownTile.Unit.StackSize -= SplitAmount;
        LandUnitEventController landUnit = mock.GetComponent<LandUnitEventController>();
        if (targetTile.Unit == null)
            landUnit.MoveToEmpty(null, new List<TileController>() { targetTile });
        else
            landUnit.MoveToMerge(null, new List<TileController>() { targetTile });
        unit.Owner.Moves--;
        ResetSplitTiles();
        return DeselectStatus.Both;
    }

    public virtual GameObject CreateSplitMock() {
        GameObject mock = Instantiate(gameObject);
        mock.name = gameObject.name;
        BaseUnit unit = mock.GetComponent<BaseUnit>();
        unit.StackSize = SplitAmount;
        unit.Owner = GetComponent<BaseUnit>().Owner;
        return mock;
    }

    protected void ResetSplitTiles() {
        if (_surrTiles == null)
            return;
        foreach (TileController tile in _surrTiles)
            tile.ResetSprite();
        _surrTiles = null;
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
		if (direction.x - position.x < 0)
			anim.SetInteger ("Direction", 3); // Left
		else if (direction.x - position.x > 0)
			anim.SetInteger ("Direction", 1); // Right
		else if (direction.y - position.y > 0)
			anim.SetInteger ("Direction", 0); // Up
		else if (direction.y - position.y < 0)
			anim.SetInteger ("Direction", 2); // Down
	}

    private void QueueNextItem() {
        GameObject.Find("Board").GetComponent<GameController>().NextQueueItem = true;
    }
}
