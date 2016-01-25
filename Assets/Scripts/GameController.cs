using System;
using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public GameObject BasePrefab;
	public int AmountOfPlayers;
	public List<Player> Players { get; private set; }
	public List<Player> AllPlayers { get; private set; }

	void Awake() {
		Players = new List<Player> ();
		AllPlayers = new List<Player> ();
		GeneratePlayers ();
	}

	void GeneratePlayers() {
	    var rnd = new System.Random();

	    List<int> ids = new List<int>() { 1, 2, 3, 4 };

	    for (int i = 0; i < AmountOfPlayers; i++) {
	        int random = rnd.Next(0, ids.Count);
	        Player player = new Player(ids[random]);
	        ids.RemoveAt(random);
	        Players.Add(player);
			AllPlayers.Add(player);

	        Board board = gameObject.GetComponent<Board>();

	        switch (player.PlayerId) {
                case 1:
	                CreateBase(player, board, 0, 0);
	                break;
                case 2:
                    CreateBase(player, board, board.BoardDimensions - 1, 0);
	                break;
                case 3:
	                CreateBase(player, board, 0, board.BoardDimensions - 1);
	                break;
                case 4:
	                CreateBase(player, board, board.BoardDimensions - 1, board.BoardDimensions - 1);
	                break;
                default:
	                throw new ArgumentOutOfRangeException("Only 4 players allowed.");
	        }

	    }
	}

    void CreateBase(Player owner, Board board, int x, int y) {
        GameObject go = board._tiles[x, y];
        TileController tile = go.GetComponent<TileController>();
        GameObject baseObject = Instantiate(BasePrefab, new Vector3(x,y), Quaternion.identity) as GameObject;
        tile.Unit = baseObject.GetComponent<BaseUnit>();
        tile.Unit.Owner = owner;
    }
		
	public void RemovePlayer(Player player) {
		Players.Remove(player);
	}
}
