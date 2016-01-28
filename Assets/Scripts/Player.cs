﻿using System;
using UnityEngine;


public class Player{

    public int PlayerId { get; private set; }
    public string Name;
    public int MoneyAmount;
    public Color Color;
    public Environment StartEnvironment;
	public int Moves;
    public bool IsCurrentPlayer { get; private set; }
	public Sprite BarrackSprite;
	public bool IsAlive = true;

    public int CalculateIncome() {
        //throw new System.NotImplementedException();
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
		GenerateMoney ();
	}

    public void EndTurn() {
        IsCurrentPlayer = false;
    }

    private void GenerateMoney() {
        MoneyAmount += CalculateIncome();
    }

	public Player(int x) {
		PlayerId = x;
		MoneyAmount = 500;
	}
}
