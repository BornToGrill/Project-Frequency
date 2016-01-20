using System;
using UnityEngine;


public class Player {

    public int PlayerId { get; private set; }
    public int MoneyAmount;
    public Color Color;
    public Environment StartEnvironment;

    void GenerateMoney() {
        throw new System.NotImplementedException();
    }


	public Player(int x) {
		PlayerId = x;
		MoneyAmount = 500;

        // TODO: Remove temporary
		switch (PlayerId) {
		case 1:
			Color = Color.red;
			break;
		case 2:
			Color = Color.gray;
			break;
		case 3:
			Color = Color.cyan;
			break;
		case 4:
			Color = Color.green;
			break;
		default:
			Color = Color.black;
			break;
		};
	}

}
