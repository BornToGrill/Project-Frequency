using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WinScreen : MonoBehaviour {

	private Player winner;
	private List<Player> losers;

	// Use this for initialization
	void Start () {
		GameController gc = GetComponent<GameController> ();
		winner = WinCondition.winner;
		losers = WinCondition.losers;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
