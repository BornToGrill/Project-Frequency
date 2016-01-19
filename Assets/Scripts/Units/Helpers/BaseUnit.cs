using UnityEngine;

public abstract class BaseUnit : MonoBehaviour {

    internal GameObject Owner;

    public int Health;
    public int Cost;
    public int DiscountCost;
    public Environment DiscountEnvironment;


    public virtual int GetCost(Environment environment) {
        return environment == DiscountEnvironment ? DiscountCost : Cost;
    }

    public abstract void DamageUnit(int damage);
}
