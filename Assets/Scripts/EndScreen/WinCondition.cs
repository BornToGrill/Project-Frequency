using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class WinCondition : MonoBehaviour {

	private Player _baseOwner;
	static public Player winner;
	static public List<Player> losers;
	public int moneyWinAmount;

	void Start () {
		BaseUnit unit = gameObject.GetComponent<BaseUnit> ();
		_baseOwner = unit.Owner;
	}
	
	// Update is called once per frame
	void Update () {
		moneyCondition (_baseOwner);
	}


	void OnDestroy() {
		GameObject boardGo = GameObject.Find ("Board");
		if (boardGo != null) {
			GameController gc = boardGo.GetComponent<GameController> ();
			gc.RemovePlayer (_baseOwner);

			if (gc.Players.Count == 1) {
				gc.AllPlayers.Remove (gc.Players[0]);
				winner = gc.Players[0];
				losers = gc.AllPlayers;

				//SceneManager.LoadScene ("WinScreen"); 
			}
		}
	}

	public bool moneyCondition(Player player) {
		if (player.MoneyAmount >= moneyWinAmount) {
			GameObject boardGo = GameObject.Find ("Board");
			if (boardGo != null) {
				GameController gc = boardGo.GetComponent<GameController> ();
				gc.AllPlayers.Remove (player);

				winner = player;
				losers = gc.AllPlayers;

				SceneManager.LoadScene ("WinScreen"); 
			} else {
				return false;
			}
		}
		return false;
	}
}
