﻿using System;
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
			SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer> ();
			switch (_environment) {
				case Environment.Swamp:
					sr.color = Color.gray;
                    _monetaryValue = 50;
					break;
				case Environment.Ice:
					sr.color = Color.white;
                    _monetaryValue = 50;
                    break;
				case Environment.Desert:
					sr.color = Color.yellow;
                    _monetaryValue = 50;
                    break;
				case Environment.Forest:
					sr.color = Color.green;
                    _monetaryValue = 50;
                    break;
				case Environment.Island:
					sr.color = Color.black;
                    _monetaryValue = 150;
                    break;
				case Environment.Water:
					sr.color = Color.blue;
                    _monetaryValue = 0;
					break;
			}	
		}
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
        BaseUnit unitBase = unit.GetComponent<TileController>().Unit;
        if (unitBase.TraversableEnvironments == null)
            throw new NullReferenceException("No traversable environment set");
        if (!unitBase.TraversableEnvironments.Contains(this.Environment))
            return false;
        if (this.Unit == null)
            return true;
        return this.Unit.name == unitBase.name && (this.Unit.StackSize + unitBase.StackSize <= this.Unit.MaxUnitStack);
    }
}