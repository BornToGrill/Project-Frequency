using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour {
    public GameObject BasePrefab;
	public int MovesPerTurn;
    public int CashWinCondition;
	public Player CurrentPlayer { get; set; }
	public List<Player> Players { get; private set; }
	public List<Player> AllPlayers { get; private set; }
	public List<Color> PlayerColors;
	public List<Sprite> Bases;
	public List<Sprite> Barracks;

    public Queue<Action> MultiplayerActionQueue = new Queue<Action>(); 

	void Awake() {
	    GameObject endGame = GameObject.Find("EndGameData");
	    if (endGame != null)
	        Destroy(endGame);
		Players = new List<Player> ();
		AllPlayers = new List<Player> ();
	    if (GetComponent<StateController>() == null) {
	        GeneratePlayers();
            CurrentPlayer = Players[0];
            CurrentPlayer.StartTurn(this);
        }
	    else {
	        SessionData lobby = GameObject.Find("Lobby Settings").GetComponent<SessionData>();
	        foreach (TempPlayer temp in lobby.Players) {
	            Player player = CreatePlayer(temp.Id);
	            player.Name = temp.Name;
	            AllPlayers.Add(player);
	            Players.Add(player);
	        }
	    }
	}

    void Start() {
        StateController cont = GetComponent<StateController>();
        if(cont != null)
            cont.ServerComs.Notify.GameLoaded();
    }

    void Update() {
        lock (MultiplayerActionQueue) {
            while (MultiplayerActionQueue.Count > 0) {
                MultiplayerActionQueue.Dequeue().Invoke();
            }
        }
    }

    public void QueueMultiplayerAction(Action action) {
        lock(MultiplayerActionQueue)
            MultiplayerActionQueue.Enqueue(action);
    }

	void GeneratePlayers() {
	    var rnd = new System.Random();

	    List<int> spawns = new List<int>() { 1, 2, 3, 4 };

	    GameObject settings = GameObject.Find("LocalGameSettings");
	    GameData data = settings.GetComponent<GameData>();
        for (int i = 0; i < data.AmountOfPlayers; i++) {
	        int random = rnd.Next(0, spawns.Count);
            int id = spawns[random];
	        Player player = CreatePlayer(id);
            spawns.RemoveAt(random);
	        Players.Add(player);
			AllPlayers.Add(player);
            
            player.Name = "P" + (i + 1);
	    }
	}

    Player CreatePlayer(int id) {
        Board board = gameObject.GetComponent<Board>();
        Player player = new Player(id);
        switch (id) {
            case 1:
                CreateBase(player, board, 0, 0, id);
                break;
            case 2:
                CreateBase(player, board, board.BoardDimensions - 1, 0, id); ;
                break;
            case 3:
                CreateBase(player, board, 0, board.BoardDimensions - 1, id);
                break;
            case 4:
                CreateBase(player, board, board.BoardDimensions - 1, board.BoardDimensions - 1, id);
                break;
            default:
                Debug.LogError("Only 4 players allowed.");
                break;
        }
        player.Color = PlayerColors[id - 1];
        player.BarrackSprite = Barracks[id - 1];
        return player;
    }

	void CreateBase(Player owner, Board board, int x, int y, int id) {
        GameObject go = board._tiles[x, y];
        TileController tile = go.GetComponent<TileController>();
        owner.StartEnvironment = tile.Environment;
        GameObject baseObject = Instantiate(BasePrefab, new Vector3(x,y), Quaternion.identity) as GameObject;
		baseObject.GetComponent<Animator> ().SetInteger ("Color", id);
		baseObject.GetComponent<Animator> ().SetInteger ("Type", 1);
        tile.Unit = baseObject.GetComponent<BaseUnit>();
        tile.Unit.Owner = owner;
    }
		

	public void RemovePlayer(Player player) {
		player.IsAlive = false;
		Players.Remove(player);
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

    public void NextTurn(int id) {
        lock (Players) {
            lock (CurrentPlayer) {
                CurrentPlayer.EndTurn();
                CurrentPlayer = Players.Find(x => x.PlayerId == id);
                CurrentPlayer.StartTurn(this);
            }
        }
    }
}
