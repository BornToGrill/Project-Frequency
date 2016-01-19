using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public int AmountOfPlayers;
	private List<Player> _players;
	public List<Player> Players {
		get { return _players; }
	}

	void GeneratePlayers() {
		for (int i = 1; i <= AmountOfPlayers; i++) {
			_players.Add(new Player(i));
		}
	}


}
