using UnityEngine;

public abstract class BaseUnit : MonoBehaviour {

    internal abstract int StackSize { get; set; }

    internal GameObject Owner;

    public int Health;
    public int Cost;
    public int DiscountCost;
    public Environment DiscountEnvironment;

    public Environment[] TraversableEnvironments;

    public int MaxUnitStack;

    public virtual int GetCost(Environment environment) {
        return environment == DiscountEnvironment ? DiscountCost : Cost;
    }

    public abstract void DamageUnit(int damage);
}
