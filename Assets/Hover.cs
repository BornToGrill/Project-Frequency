using UnityEngine;
using System.Collections;

public class Hover : MonoBehaviour {


	public void ActivateHover(GameObject go) {
		Node node = go.GetComponent<Node> ();
		Debug.Log (node.X.ToString () + " : " + node.Y.ToString ());
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
