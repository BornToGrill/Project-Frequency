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
            controller.Environment = controller.Environment;
        _currentlyModified.Clear();

        if(firstTile == secondTile)
            return DeselectStatus.Both;

        TileController first = firstTile.GetComponent<TileController>();
        TileController second = secondTile.GetComponent<TileController>();

        var path = Pathfinding.FindPath(first, second);
        // if (path.Count > Owner.RemainingMoves) // TODO: Remainin moves check.
        //    return DeselectStatus.Both;

        if (second.Unit == null)
            return MoveToEmpty(path);
        else {
            if (second.Unit.Owner != Owner)
                return MoveToAttack(path);
            else {
                if (second.IsTraversable(first.gameObject))
                    return MoveToMerge(path);
                return DeselectStatus.Both;
            }
        }
    }

    public virtual DeselectStatus MoveToEmpty(List<TileController> path) {

        // Loop over path and move to each tile.
        // On each move remove self from last position.

        TileController previous = path[0];
        for (int i = 1; i < path.Count; i++) {
            previous.Unit = null;
            path[i].Unit = this;
            path[i].Unit.gameObject.transform.position = path[i].transform.position;
            previous = path[i];
        }
        return DeselectStatus.Both;
    }

    public virtual DeselectStatus MoveToAttack(List<TileController> path) {
        // Move to path - Range tile.
        // Attack.

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

    public virtual DeselectStatus MoveToMerge(List<TileController> path) {
        TileController previous = path[0];
        for (int i = 0; i < path.Count - 1; i++) {
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
        List<TileController> path = Pathfinding.FindPath(first, second);
        // TODO: Check for remaining moves.

        if (second.Unit == null || second.IsTraversable(first.gameObject)) {
            _currentlyModified = path.GetRange(1, path.Count - 1);
            for (int i = 0; i < _currentlyModified.Count; i++) {
                SpriteRenderer render = _currentlyModified[i].gameObject.GetComponent<SpriteRenderer>();
                if (i == _currentlyModified.Count)
                    render.color = SelfSelectedColor;
                else
                    render.color = MoveColor;
            }
        }
        else {
            if (second.Unit.Owner != Owner) {
                _currentlyModified = path.GetRange(1, path.Count - 1);
                for (int i = 0; i < _currentlyModified.Count - Range; i++) {
                    SpriteRenderer render = _currentlyModified[i].gameObject.GetComponent<SpriteRenderer>();
                    render.color = MoveColor;
                }
                second.GetComponent<SpriteRenderer>().color = AttackColor;
            }
            else {
                _currentlyModified = path.GetRange(1, path.Count - 1);
                foreach (TileController element in _currentlyModified)
                    element.gameObject.GetComponent<SpriteRenderer>().color = InvalidMoveColor;
            }
        }

    }

    public override void OnMouseLeave(GameObject firstTile, GameObject secondTile) {
        foreach (TileController element in _currentlyModified)
            element.Environment = element.Environment;
        _currentlyModified.Clear();
    }

    #endregion
}
