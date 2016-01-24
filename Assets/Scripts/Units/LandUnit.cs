using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LandUnit : BaseUnit {

    public Color MoveColor;
    public Color AttackColor;
    public Color InvalidMoveColor;

    public int Damage;
    public int Range;

    private int _stackSize;
    private int _stackHealth;
    private int _stackDamage;

    private List<TileController> _currentlyModified = new List<TileController>(); 

    internal override int StackSize {
        get { return _stackSize; }
        set {
            int diff = Mathf.Abs(value - _stackSize);
            _stackHealth += diff * Health;
            _stackDamage += diff * Damage;
            _stackSize = value;
        }
    }

    public bool CanAttack(BaseUnit unit) {
        return unit.Owner != this.Owner;
    }

    public void Attack(BaseUnit unit) {
        if (unit.Owner != this.Owner)
            unit.DamageUnit(_stackDamage);
    }

    public override void DamageUnit(int damage) {
        _stackHealth -= damage;
        if (_stackHealth <= 0) {
            GameObject.Destroy(gameObject);
            return;
        }
        _stackSize = Mathf.CeilToInt((float)_stackHealth / Health);
        _stackDamage = Damage * _stackSize;
    }

    #region Selection
    public override DeselectStatus OnFirstSelected(GameObject firstTile) {
        firstTile.GetComponent<SpriteRenderer>().color = SelfSelectedColor;
        return DeselectStatus.None;
    }

    public override DeselectStatus OnSecondClicked(GameObject firstTile, GameObject secondTile) {
        foreach (TileController controller in _currentlyModified)
            controller.ResetSprite();
        firstTile.GetComponent<TileController>().ResetSprite();

        _currentlyModified.Clear();

        if(firstTile == secondTile)
            return DeselectStatus.Both;

        TileController first = firstTile.GetComponent<TileController>();
        TileController second = secondTile.GetComponent<TileController>();

        PathFindingResult path = Pathfinding.FindPath(first, second);
        // if (path.Count > Owner.RemainingMoves) // TODO: Remainin moves check.
        //    return DeselectStatus.Both;

        if (second.Unit == null)
            return MoveToEmpty(first, path.Path);
        else {
            if (second.Unit.Owner != Owner)
                return MoveToAttack(first, path.Path);
            else {
                if (second.IsTraversable(gameObject))
                    return MoveToMerge(first, path.Path);
                return DeselectStatus.Both;
            }
        }
    }

    public virtual DeselectStatus MoveToEmpty(TileController start, List<TileController> path) {

        // Loop over path and move to each tile.
        // On each move remove self from last position.
        // TODO: Take into account on all moves that Path list now does not contain own position anymore!
        start.Unit = null;
        TileController previous = path[0];
        for (int i = 1; i < path.Count; i++) {
            previous.Unit = null;
            path[i].Unit = this;
            path[i].Unit.gameObject.transform.position = path[i].transform.position;
            previous = path[i];
        }
        return DeselectStatus.Both;
    }

    public virtual DeselectStatus MoveToAttack(TileController start, List<TileController> path) {
        // Move to path - Range tile.
        // Attack.
        if(path.Count - Range > 0)
            start.Unit = null;
        TileController previous = path[0];
        for (int i = 1; i < path.Count - Range; i++) {
            previous.Unit = null;
            path[i].Unit = this;
            path[i].Unit.gameObject.transform.position = path[i].transform.position;
            previous = path[i];
        }
        path.Last().Unit.DamageUnit(_stackDamage);
        return DeselectStatus.Both;
    }

    public virtual DeselectStatus MoveToMerge(TileController start, List<TileController> path) {
        start.Unit = null;

        TileController previous = path[0];
        for (int i = 1; i < path.Count - 1; i++) {
            previous.Unit = null;
            path[i].Unit = this;
            path[i].Unit.gameObject.transform.position = path[i].transform.position;
            previous = path[i];
        }
        GameObject.Destroy(previous.Unit.gameObject);
        path.Last().Unit.StackSize += this.StackSize;

        return DeselectStatus.Both;
    }
    #endregion

    #region Mouse Enter/Leave

    public override void OnMouseEnter(GameObject firstTile, GameObject secondTile) {
        if (firstTile == secondTile)
            return;
        TileController first = firstTile.GetComponent<TileController>();
        TileController second = secondTile.GetComponent<TileController>();
        PathFindingResult path = Pathfinding.FindPath(first, second);
        _currentlyModified = path.Path;

        // TODO: Check for remaining moves.
        if (path.FoundEndPoint == false || !path.ValidPath)
            foreach (var element in _currentlyModified)
                element.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
        else {
            if (second.Unit == null) {
                if (second.IsTraversable(gameObject))
                    foreach (var element in _currentlyModified)
                        element.GetComponent<SpriteRenderer>().color = MoveColor;
                else
                    foreach (var element in _currentlyModified)
                        element.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
            }

            else {
                if (second.Unit.Owner != Owner) {
                    for (int i = 0; i < _currentlyModified.Count - Range; i++)
                        _currentlyModified[i].GetComponent<SpriteRenderer>().color = MoveColor;
                    _currentlyModified.Last().GetComponent<SpriteRenderer>().color = AttackColor;
                }
                else
                    foreach (var element in _currentlyModified)
                        element.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
            }
        }
    }

    public override void OnMouseLeave(GameObject firstTile, GameObject secondTile) {
        foreach (TileController element in _currentlyModified)
            element.ResetSprite();
        _currentlyModified.Clear();
    }

    #endregion
}
