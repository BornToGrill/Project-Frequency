using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player {

    private int _moneyAmount;
    private StateController _multiplayerController;

    public int PlayerId { get; private set; }
    public string Name;
    public Color Color;
    public Environment StartEnvironment;
	public int Moves;
    public bool IsCurrentPlayer { get; private set; }
	public Sprite BarrackSprite;
	public bool IsAlive = true;

    public int MoneyAmount {
        get { return _moneyAmount; }
        set {
            _moneyAmount = value;
            if(_multiplayerController != null &&
                _multiplayerController.CornerId == PlayerId &&
                _multiplayerController.ServerComs != null && _multiplayerController.ServerComs.Notify != null)
                _multiplayerController.ServerComs.Notify.CashChanged(value);
        }
    }

    public int CalculateIncome() {
        Board board = GameObject.Find("Board").GetComponent<Board>();
        int income = 0;
        foreach(GameObject tileObject in board._tiles)
        {
            TileController controller = tileObject.GetComponent<TileController>();
            if (controller.Unit != null && controller.Unit.Owner == this)
            {
                income += controller.GetMonetaryValue(StartEnvironment);
            }
        }
        return income;
    }

	public void StartTurn(GameController gameController) {
        IsCurrentPlayer = true;
		Moves = gameController.MovesPerTurn;

	    if (_multiplayerController == null) {
            GenerateMoney();
            GameController gc = GameObject.Find("Board").GetComponent<GameController>();
            if (MoneyAmount >= gc.CashWinCondition) {
                WinCondition.Winner = this;
                WinCondition.Losers = gc.AllPlayers.Where(x => x != this).ToArray();

                SceneManager.LoadScene("WinScreen");   
            }
        }
	    else {
	        if (_multiplayerController.CornerId == PlayerId) {
                GenerateMoney();
	        }
	    }
	}

    public void EndTurn() {
        IsCurrentPlayer = false;
    }

    private void GenerateMoney() {
        MoneyAmount += CalculateIncome();
    }

	public Player(int x) {
        _multiplayerController = GameObject.Find("Board").GetComponent<StateController>();
        PlayerId = x;
		MoneyAmount = 500;
	}
}
