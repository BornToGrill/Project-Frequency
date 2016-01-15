using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	private Environment _environment;

	public Tile Left;
	public Tile Right;
	public Tile Up;
	public Tile Down;
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
}
