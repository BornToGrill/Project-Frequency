using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Player {

    public int Number;
    public int MoneyAmount;
    public Color Color;
    public Environment StartEnvironment;
	public List<GameObject> units;

    private void GenerateMoney() {
        //throw new System.NotImplementedException();
        Board board = GameObject.Find("Board").GetComponent<Board>();
        foreach(GameObject tileObject in board._tiles)
        {
            TileController controller = tileObject.GetComponent<TileController>();
            if (controller.Unit != null && controller.Unit.Owner == this)
            {
                MoneyAmount += controller.GetMonetaryValue(StartEnvironment);
            }
        }


    }


	public Player(int x) {
		Number = x;
		MoneyAmount += 500;

		switch (Number) {
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
