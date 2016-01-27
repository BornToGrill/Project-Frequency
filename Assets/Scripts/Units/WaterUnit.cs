using System;
using System.Linq;
using UnityEngine;

public class WaterUnit : LandUnit {


    private Environment[] _defaultEnvironments;

    internal override int StackSize {
        get { return 1; }
        set {
            if (value != 1)
                throw new InvalidOperationException("Only 1 water unit allowed per tile");
        }
    }

    internal GameObject CarryUnit;

    public override void Awake() {
        //StackSize = 1;
        base.Awake();
        _defaultEnvironments = TraversableEnvironments;

        //if ((GameObject.Find("Board").GetComponent<StateController>() == null)) {
        //    CurrentPlayerPredicate = (x) => { return x.Unit.Owner.IsCurrentPlayer; };
        //}
        //else {
        //    GameObject board = GameObject.Find("Board");
        //    StateController state = board.GetComponent<StateController>();
        //    GameController game = board.GetComponent<GameController>();
        //    CurrentPlayerPredicate = (x) => {
        //        return x.Unit.Owner.IsCurrentPlayer && x.Unit.Owner.PlayerId == state.CornerId;
        //    };
        //}
    }

    public override bool CanMerge(BaseUnit unit) {
        if (CarryUnit == null)
            return true;
        BaseUnit internalUnit = CarryUnit.GetComponent<BaseUnit>();
        return internalUnit.Owner == unit.Owner && CarryUnit.gameObject.name == unit.gameObject.name && internalUnit.StackSize + unit.StackSize < internalUnit.MaxUnitStack;
    }

    public override void Merge(BaseUnit unit) {
        if (CarryUnit == null) {
            CarryUnit = unit.gameObject;
            CarryUnit.SetActive(false);
            TraversableEnvironments = _defaultEnvironments.Concat(unit.TraversableEnvironments).ToArray();
        }
        else {
            CarryUnit.GetComponent<BaseUnit>().StackSize += unit.StackSize;
            GameObject.Destroy(unit.gameObject);
        }

    }

    public void UnloadUnit(GameObject tile) {
        CarryUnit.SetActive(true);
        CarryUnit.transform.position = gameObject.transform.position;
        CarryUnit = null;
        TraversableEnvironments = _defaultEnvironments;
    }

    public override void DamageUnit(int damage) {
        Health -= damage;
        if (Health > 0)
            return;
        Health = 0;
        GameObject.Destroy(gameObject);
    }
}
