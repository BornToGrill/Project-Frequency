using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public enum Environment {Swamp, Ice, Desert, Forest, Water, Island, None};

public class TileController : MonoBehaviour {
    private Environment _environment;
    internal BaseUnit Unit;
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
					break;
				case Environment.Ice:
					sr.color = Color.white;
					break;
				case Environment.Desert:
					sr.color = Color.yellow;
					break;
				case Environment.Forest:
					sr.color = Color.green;
					break;
				case Environment.Island:
					sr.color = Color.black;
					break;
				case Environment.Water:
					sr.color = Color.blue;
					break;
				case Environment.None:
					sr.color = Color.clear;
					break;
			}	
		}
	}

    public bool IsTraversable(GameObject unit) {
        BaseUnit unitBase = unit.GetComponent<BaseUnit>();

        if (unitBase.TraversableEnvironments == null)
            throw new NullReferenceException("No traversable environment set");
        if (!unitBase.TraversableEnvironments.Contains(this.Environment))
            return false;
        if (this.Unit == null)
            return true;
        return gameObject.name == unit.name && (unitBase.StackSize + unitBase.StackSize < unitBase.MaxUnitStack);
    }
}