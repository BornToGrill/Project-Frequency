using UnityEngine;
using System.Collections;

public class StatusBarController : MonoBehaviour {
	private GameController _gameController;
	private StatusBarPlayerField[] _playerFields;

	public GameObject board;
	public GameObject StatusBarPlayerPrefab;

	void Start() {
		_gameController = board.GetComponent<GameController> ();
		_playerFields = new StatusBarPlayerField[_gameController.AmountOfPlayers];

		for (int i = 0; i < _gameController.AmountOfPlayers; i++) {
			GameObject p = Instantiate (StatusBarPlayerPrefab, new Vector3 (0, 0), new Quaternion ()) as GameObject;
			StatusBarPlayerField playerField = p.GetComponent<StatusBarPlayerField> ();
			playerField.Initialize (gameObject.GetComponent<RectTransform>(), _gameController.Players [i]);
			_playerFields [i] = playerField;
		}
	}

	void Update() {
		UpdateStats ();
	}

	void UpdateStats() {
		foreach (StatusBarPlayerField p  in _playerFields) {
			p.UpdateStats ();
		}
	}
}
