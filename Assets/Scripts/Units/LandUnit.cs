using UnityEngine;

public class LandUnit : BaseUnit {
    public int Damage;
    public int Range;

    private int _stackSize;
    private int _stackHealth;
    private int _stackDamage;


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
        // TODO: Death check for all and individual units in stack.
    }

    public override DeselectStatus OnFirstSelected(GameObject firstTile) {
        // TODO: Highlight own tile.
        throw new System.NotImplementedException();
    }

    public override DeselectStatus OnSecondClicked(GameObject firstTile, GameObject secondTile) {
        var path = AStarPathfinding.FindPath(firstTile.GetComponent<Node>(), secondTile.GetComponent<Node>());
        //if(Owner.AvailableMoves < path.Count)
        //    return DeselectStatus.Both;
        TileController second = secondTile.GetComponent<TileController>();
        if (second.Unit == null) {
            // Loop over path and move to each tile.
            // On each move remove self from last position.
            return DeselectStatus.Both;
        }
        else {
            if (second.Unit.Owner != Owner) {
                // Move to path - 1 tile.
                // Attack.
                return DeselectStatus.Both;
            }
            else {
                if (second.Unit is LandUnit && ((LandUnit) second.Unit).StackSize + StackSize < second.Unit.MaxUnitStack) {
                    // Move to stack.
                    // Increment stack.
                    // Remove self from previous
                }
                return DeselectStatus.Both;
            }
        }
    }
}
