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

    void Start() {
        ComHandler = new CommunicationHandler(null, null, this);
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

    public void Authenticated(string guid, int id) {
        lock (_lobbyActions)
            _lobbyActions.Enqueue(() => {
                GameObject lobbySettings = GameObject.Find("Lobby Settings");
                if (lobbySettings == null) {
                    lobbySettings = new GameObject("Lobby Settings");
                    lobbySettings.AddComponent<SessionData>();
                    DontDestroyOnLoad(lobbySettings);
                }
                SessionData session = lobbySettings.GetComponent<SessionData>();
                session.Guid = guid;
                session.OwnId = id;
                ComHandler.SetGuid(guid);
            });
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
