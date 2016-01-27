using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour, ILobby {

    private Queue<Action> _lobbyActions = new Queue<Action>();

    internal CommunicationHandler ComHandler;
    public Text[] PlayerFields;

	void Start () {
	    ComHandler = new CommunicationHandler(null, null, this);
	}

    void Update() {
        lock(_lobbyActions)
            while (_lobbyActions.Count > 0)
                _lobbyActions.Dequeue().Invoke();
    }

    #region ILobby Implementation Members

    public void PlayerJoined(int id, string playerName) {

        lock (_lobbyActions)
            _lobbyActions.Enqueue(() => {
                foreach (Text field in PlayerFields) {
                    if (string.IsNullOrEmpty(field.text)) {
                        field.text = playerName;
                        break;
                    }
                }
            });
    }

    public void PlayerLeft(int id, string name) {
        //throw new System.NotImplementedException();
    }

    public void GameStart() {
        //throw new System.NotImplementedException();
    }
    #endregion
}
