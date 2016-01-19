using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {

	private bool switcher;

	public void ActivateHover() {
		AStarPathfinding.EndLocation = gameObject.GetComponent<Node> ();
		gameObject.GetComponent<SpriteRenderer> ().color = Color.black;
	}

	public void ExitHover() {
		gameObject.GetComponent<TileController> ().Environment = gameObject.GetComponent<TileController> ().Environment;
	}

	public void ActivateClick() {
		AStarPathfinding.StartLocation = gameObject.GetComponent<Node> ();
		gameObject.GetComponent<SpriteRenderer> ().color = Color.black;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
