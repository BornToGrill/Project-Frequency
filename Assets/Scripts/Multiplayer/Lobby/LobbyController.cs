using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour, ILobby {

    private Queue<Action> _lobbyActions = new Queue<Action>();

    internal CommunicationHandler ComHandler;
    public Text[] PlayerFields;
    public Text LobbyId;

    void Start() {
        GameObject go = GameObject.Find("Lobby Settings");
        SessionData session = go.GetComponent<SessionData>();
        LobbyId.text = session.LobbyId;
        for (int i = 0; i < session.Players.Length + 1; i++) {
            if (i < session.Players.Length)
                PlayerFields[i].text = session.Players[i].Name;
            else
                PlayerFields[i].text = session.OwnName;
        }

        ComHandler = new CommunicationHandler(session.LobbyConnection, null, null, this);
        ComHandler.SetGuid(session.Guid);
    }

    void Update() {
        lock (_lobbyActions)
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
        lock (_lobbyActions)
            _lobbyActions.Enqueue(() => {
                foreach (Text field in PlayerFields) {
                    if (field.text == name) {
                        field.text = "";
                        break;
                    }
                }
            });

    }

    public void SetPlayers(string[] names) {
        lock (_lobbyActions) {
            _lobbyActions.Enqueue(() => {
                for (int i = 0; i < PlayerFields.Length; i++) {
                    PlayerFields[i].text = i < names.Length ? names[i] : "";
                }
            });
        }
    }

    public void GameStart(TempPlayer[] players) {
        lock (_lobbyActions)
            _lobbyActions.Enqueue(() => {
                GameObject lobbySettings = GameObject.Find("Lobby Settings");
                SessionData session = lobbySettings.GetComponent<SessionData>();
                session.Players = players;
                session.ServerCom = ComHandler;

                SceneManager.LoadScene("MultiplayerGame");
            });
    }

    #endregion
}
