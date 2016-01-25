using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour {
    public GameObject BasePrefab;
	public int AmountOfPlayers;
	public int MovesPerTurn;
	public Player CurrentPlayer { get; private set; }
	public List<Player> Players { get; private set; }

	void Awake() {
		Players = new List<Player> ();
		GeneratePlayers ();
		CurrentPlayer = Players [0];
		CurrentPlayer.StartTurn (this);
	}

	void GeneratePlayers() {
	    var rnd = new System.Random();

	    List<int> spawns = new List<int>() { 1, 2, 3, 4 };

	    for (int i = 0; i < AmountOfPlayers; i++) {
	        int random = rnd.Next(0, spawns.Count);
	        Player player = new Player(i + 1);
            int id = spawns[random];
	        spawns.RemoveAt(random);
	        Players.Add(player);
            player.Name = "P" + (i + 1);

	        Board board = gameObject.GetComponent<Board>();

	        switch (id) {
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
        owner.StartEnvironment = tile.Environment;
        GameObject baseObject = Instantiate(BasePrefab, new Vector3(x,y), Quaternion.identity) as GameObject;
        tile.Unit = baseObject.GetComponent<BaseUnit>();
        tile.Unit.Owner = owner;
    }
		
	public void NextTurn() {
		int i = Players.IndexOf (CurrentPlayer);
		i += 1;
		if (i >= Players.Count)
			i = 0;
        CurrentPlayer.EndTurn();
		CurrentPlayer = Players [i];
		CurrentPlayer.StartTurn (this);
	}
}