using System;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour {

    internal abstract int StackSize { get; set; }

    internal Player Owner;

    internal Predicate<TileController> CurrentPlayerPredicate; 

    public int Health;
    public int Cost;
    public int DiscountCost;
    public Environment DiscountEnvironment;

    public Environment[] TraversableEnvironments;

    public int MaxUnitStack;

    public virtual void Awake() {
        StackSize = 1;
        if ((GameObject.Find("Board").GetComponent<StateController>() == null)) {
            CurrentPlayerPredicate = (x) => { return x.Unit.Owner.IsCurrentPlayer; };
        }
        else {
            GameObject board = GameObject.Find("Board");
            StateController state = board.GetComponent<StateController>();
            GameController game = board.GetComponent<GameController>();
            CurrentPlayerPredicate = (x) => {
                return x.Unit.Owner.IsCurrentPlayer && x.Unit.Owner.PlayerId == state.CornerId;
            };
        }
    }

    public virtual int GetCost(Environment environment) {
        return environment == DiscountEnvironment ? DiscountCost : Cost;
    }

    public abstract void DamageUnit(int damage);

}
