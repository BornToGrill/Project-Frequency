using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {

	private bool switcher;

	public void ActivateHover() {
		AStarPathfinding.EndLocation = gameObject.GetComponent<Node> ();
	}

	public void ActivateClick() {
			AStarPathfinding.StartLocation = gameObject.GetComponent<Node> ();
			
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
