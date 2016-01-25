using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class winCondition : MonoBehaviour {

	private Player _baseOwner;

	// Use this for initialization
	void Start () {
		BaseUnit bu = gameObject.GetComponent<BaseUnit> ();
		_baseOwner = bu.Owner;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnDestroy() {
		GameObject boardGo = GameObject.Find ("Board");
		GameController gc = boardGo.GetComponent<GameController> ();
		gc.RemovePlayer (_baseOwner);

		if (gc.Players.Count == 1) {
			Debug.Log ("Player " + gc.Players [0].PlayerId + " has won the game");
			SceneManager.LoadScene("WinScreen"); 
		}
	}
}
