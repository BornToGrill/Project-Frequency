using UnityEngine;
using System.Collections;

public class winCondition : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void onDestroy() {
		BaseUnit bu = gameObject.transform.GetComponent<BaseUnit> ();
		GameController gc = gameObject.transform.GetComponent<GameController> ();

		Debug.Log (gc.Players.Count);
	}
}
