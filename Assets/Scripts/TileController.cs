using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public enum Environment {Swamp, Ice, Desert, Forest, Water, Island};

public class TileController : MonoBehaviour {

    private Environment _environment;

    internal BaseUnit Unit;

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