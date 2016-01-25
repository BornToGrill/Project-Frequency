using UnityEngine;
using System.Collections;

public class WinScreen : MonoBehaviour {

	//public List<Player> Players { get; private set; }
	public GUIText Wtext;

	// Use this for initialization
	void Start () {
		Wtext.text = "Hallo ik ben bert en ik <3 hagrid>gogogo";
		Wtext.transform.position = new Vector3 (0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
