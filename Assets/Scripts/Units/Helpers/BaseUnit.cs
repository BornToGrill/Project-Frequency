using UnityEngine;

public abstract class BaseUnit : MonoBehaviour {

    internal abstract int StackSize { get; set; }

    internal Player Owner;

    public int Health;
    public int Cost;
    public int DiscountCost;
    public Environment DiscountEnvironment;

    public Environment[] TraversableEnvironments;

    public int MaxUnitStack;

    void Awake() {
        StackSize = 1;
    }

    public virtual int GetCost(Environment environment) {
        return environment == DiscountEnvironment ? DiscountCost : Cost;
    }

    public abstract void DamageUnit(int damage);

    public abstract DeselectStatus OnFirstSelected(GameObject firstTile);
    public abstract DeselectStatus OnSecondClicked(GameObject firstTile, GameObject secondTile);
}
