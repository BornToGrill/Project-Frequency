using System;
using System.Linq;
using UnityEngine;

public enum Environment {Swamp, Ice, Desert, Forest, Water, Island};

public class TileController : MonoBehaviour {

    private int _monetaryValue;
    private Environment _environment;

    public BaseUnit Unit;
	internal TileController Up, Down, Left, Right;
	internal Vector2 Position;

    

    public Environment Environment {
		get {return _environment;}
		set {
			_environment = value;
			switch (_environment) {
				case Environment.Swamp:
				case Environment.Ice:
				case Environment.Desert:
				case Environment.Forest:
                    _monetaryValue = 50;
                    break;
				case Environment.Island:
                    _monetaryValue = 150;
                    break;
				case Environment.Water:
                    _monetaryValue = 0;
					break;
			}
		}
	}

    void Awake() {
        ResetSprite();
    }

    public void ResetSprite() {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 0.5f); // 50% transparency
    }

    public int GetMonetaryValue(Environment playerEnvironment) {
        if (playerEnvironment == Environment || Environment == Environment.Island)
            return _monetaryValue;
        return _monetaryValue * 2;
    }

    public bool IsTraversable() {
        return Unit == null;
	}

    public bool IsTraversable(GameObject unit) {
        BaseUnit unitBase = unit.GetComponent<BaseUnit>();
        if (unitBase.TraversableEnvironments == null)
            throw new NullReferenceException("No traversable environment set");
        if (!unitBase.TraversableEnvironments.Contains(this.Environment))
            return false;
        if (this.Unit == null)
            return true;
        return this.Unit.name == unitBase.name && (this.Unit.StackSize + unitBase.StackSize <= this.Unit.MaxUnitStack) && this.Unit.Owner == unitBase.Owner;
    }

	public bool IsTraversableUnitOnly(GameObject unit) {
		BaseUnit unitBase = unit.GetComponent<TileController>().Unit;
		if (this.Unit == null)
			return true;
		return this.Unit.name == unitBase.name && (this.Unit.StackSize + unitBase.StackSize <= this.Unit.MaxUnitStack) && this.Unit.Owner == unitBase.Owner;
	}
}