﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatusBarController : MonoBehaviour {
	private GameController _gameController;
	private List<StatusBarPlayerField> _playerFields;

	public GameObject board;
	public GameObject StatusBarPlayerPrefab;

	void Start() {
		_gameController = board.GetComponent<GameController> ();
		_playerFields = new List<StatusBarPlayerField> ();

		for (int i = 0; i < _gameController.AmountOfPlayers; i++) {
			AddPlayer(_gameController.Players [i]);
		}
	}

	void Update() {
		UpdateStats ();
	}

	public void AddPlayer(Player player) {
		float offset = 0f;
		foreach (StatusBarPlayerField existingField in _playerFields) {
			RectTransform rt = existingField.transform as RectTransform;
			offset += rt.rect.width;
		}

		GameObject fieldGo = Instantiate (StatusBarPlayerPrefab, new Vector3 (0, 0), new Quaternion ()) as GameObject;
		fieldGo.transform.SetParent (this.transform);
		StatusBarPlayerField playerField = fieldGo.GetComponent<StatusBarPlayerField> ();
		playerField.Initialize (player, offset, this);
		_playerFields.Add (playerField);
	}

	public void Clear() {
		foreach (StatusBarPlayerField p in _playerFields) {
			GameObject.Destroy (p.gameObject);
		}
		_playerFields.Clear ();
	}

	public void DeletePlayerField(StatusBarPlayerField playerField) {
		_playerFields.Remove (playerField);
		GameObject.Destroy (playerField.gameObject);

		float offset = 0;
		foreach (StatusBarPlayerField existingField in _playerFields) {
			RectTransform rt = existingField.transform as RectTransform;
			rt.anchoredPosition = new Vector2 (offset, -10.0f);
			offset += rt.rect.width;
		}
	}

	void UpdateStats() {
		foreach (StatusBarPlayerField p  in _playerFields) {
			p.UpdateStats ();
		}
	}
}