using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveCounterController : MonoBehaviour {

	public GameController gameController;
	public Text MovesAmount;

	// Use this for initialization
	void Start () {
		MovesAmount.text = gameController.CurrentPlayer.Moves.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		MovesAmount.text = gameController.CurrentPlayer.Moves.ToString();
	}
}
