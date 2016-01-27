using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LandUnitEventController : EventControllerBase {

    public Color MoveColor;
    public Color InvalidMoveColor;
    public Color AttackColor;

    public float MovementSpeed;


    public override DeselectStatus OnSelected(GameObject ownTile) {
        ownTile.GetComponent<SpriteRenderer>().color = SelfSelectedColor;
		UnitStats unitStats = GameObject.Find("UnitStats").GetComponent<UnitStats>();
		unitStats.Set (GetComponent<LandUnit> ()._stackDamage, GetComponent<LandUnit> ()._stackHealth);
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
        if (!tileOne.Unit.Owner.IsCurrentPlayer)
            return DeselectStatus.Both;
        PathFindingResult path = Pathfinding.FindPath(tileOne, tileTwo);
		Player owner = GetComponent<BaseUnit> ().Owner;

		if (tileTwo.Unit == null) {
			if (!tileTwo.IsTraversable(gameObject))
				return DeselectStatus.Both;
			if (path.Path.Count > owner.Moves)
				return DeselectStatus.Both;
			else {
				owner.Moves -= path.Path.Count;
				return MoveToEmpty (tileOne, path.Path);
			}
		}
        else {
			if (tileTwo.Unit.Owner != GetComponent<BaseUnit> ().Owner) {
				if (path.Path.Count - GetComponent<LandUnit> ().Range + 1 > owner.Moves)
					return DeselectStatus.Both;
				else {
					owner.Moves -= (path.Path.Count - GetComponent<LandUnit> ().Range + 1);
					return MoveToAttack (tileOne, path.Path);
				}
			}
            else {
                if (tileTwo.IsTraversable(gameObject))
					if (path.Path.Count <= owner.Moves)
                    	return MoveToMerge(tileOne, path.Path);
                return DeselectStatus.Both;
            }
        }
    }

    public override void OnMouseEnter(GameObject ownTile, GameObject hoveredTile) {
        if (ownTile == hoveredTile)
            return;      
        TileController tileOne = ownTile.GetComponent<TileController>();
        TileController tileTwo = hoveredTile.GetComponent<TileController>();
        if (!tileOne.Unit.Owner.IsCurrentPlayer)
            return;
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
        start.Unit = null;
        path.Last().Unit = GetComponent<BaseUnit>();
        return DeselectStatus.Both;
    }

    public virtual DeselectStatus MoveToAttack(TileController start, List<TileController> path) {
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

    public virtual DeselectStatus MoveToMerge(TileController start, List<TileController> path) {
        start.Unit = null;

        StartCoroutine(AnimateToTile(path, () => {
            ((LandUnit) path.Last().Unit).Merge(GetComponent<BaseUnit>());
        }));

        return DeselectStatus.Both;
    }
    #endregion

    internal IEnumerator AnimateToTile(IEnumerable<TileController> path) {
        yield return AnimateToTile(path, null);
    }
    internal IEnumerator AnimateToTile(IEnumerable<TileController> path, Action endAction) {
		
        foreach (TileController tile in path) {
			StartSpriteAnimation (tile.transform.position, transform.position, true);
            Vector3 startPosition = transform.position;
            for (float i = 0.1f; i <= 1f * MovementSpeed; i += 0.1f) {
                transform.position = Vector3.Lerp(startPosition, tile.transform.position, i);
                yield return null;
            }
			StartSpriteAnimation (tile.transform.position, startPosition, false);
            transform.position = tile.transform.position;
        }
        if (endAction != null)
            endAction.Invoke();
    }

	private void StartSpriteAnimation(Vector3 direction, Vector3 position, bool moving) {
		Animator anim = GetComponent<Animator> ();
		if (moving) {
			if (direction.x - position.x < 0)
				anim.Play ("MoveLeft");
			else if (direction.x - position.x > 0)
				anim.Play ("MoveRight");
			else if (direction.y - position.y > 0)
				anim.Play ("MoveUp");
			else if (direction.y - position.y < 0)
				anim.Play ("MoveDown");
			
		} else {
			if (direction.x - position.x < 0)
				anim.Play ("FaceLeft");
			else if (direction.x - position.x > 0)
				anim.Play ("FaceRight");
			else if (direction.y - position.y > 0)
				anim.Play ("FaceUp");
			else if (direction.y - position.y < 0)
				anim.Play ("FaceDown");
		}
	}
}
