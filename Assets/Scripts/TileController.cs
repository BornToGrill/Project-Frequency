﻿using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public enum Environment {Swamp, Ice, Desert, Forest, Water, Island};

public class TileController : MonoBehaviour {

    private Environment _environment;
    internal int _moneyValue;
    internal BaseUnit Unit;

    public Environment Environment {
		get {return _environment;}
		set {
			_environment = value;
			SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer> ();
			switch (_environment) {
				case Environment.Swamp:
					sr.color = Color.gray;
                    _moneyValue = 50;
					break;
				case Environment.Ice:
					sr.color = Color.white;
                    _moneyValue = 50;
                    break;
				case Environment.Desert:
					sr.color = Color.yellow;
                    _moneyValue = 50;
                    break;
				case Environment.Forest:
					sr.color = Color.green;
                    _moneyValue = 50;
                    break;
				case Environment.Island:
					sr.color = Color.black;
                    _moneyValue = 150; 
                    break;
				case Environment.Water:
					sr.color = Color.blue;
                    _moneyValue = 0;
                    break;
			}
		}
	}

    public bool GetTraversable() {
        return Unit == null;
    }

    public bool GetTraversable(BaseUnit unit) {
        if (unit.TraversableEnvironments == null)
            throw new NullReferenceException("No traversable environment set");
        if (!unit.TraversableEnvironments.Contains(this.Environment))
            return false;
        if (this.Unit == null)
            return true;
        return this.Unit.GetType() == unit.GetType() && (unit.StackSize < unit.MaxUnitStack);
    }


}