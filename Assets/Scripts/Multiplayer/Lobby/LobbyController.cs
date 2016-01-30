using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour, ILobby {

    private Queue<Action> _lobbyActions = new Queue<Action>();

    internal CommunicationHandler ComHandler;
    public Text HostField;
    public Text[] PlayerFields;
    public GameObject[] PlayerReadyObjects;

    public Text LobbyId;

    public Button ReadyStartButton;

    void Start() {
        GameObject go = GameObject.Find("Lobby Settings");
        SessionData session = go.GetComponent<SessionData>();
        LobbyId.text = session.LobbyId;
        SetPlayers(session.Players);

        ComHandler = new CommunicationHandler(session.LobbyConnection, null, null, this);
        ComHandler.SetGuid(session.Guid);
        ComHandler.SendTcp("[Request:PlayerList]");
    }

    void Update() {
        lock (_lobbyActions)
            while (_lobbyActions.Count > 0)
                _lobbyActions.Dequeue().Invoke();
    }

    #region ILobby Implementation Members


    public void SetPlayers(TempPlayer[] players) {
        //TODO: Set host & ready status
        lock (_lobbyActions) {
            _lobbyActions.Enqueue(() => {
                SessionData session = GameObject.Find("Lobby Settings").GetComponent<SessionData>();
                TempPlayer host = players.FirstOrDefault(x => x.IsHost);
                if(host != null)
                    HostField.text = host.Name;
                TempPlayer[] nonHosts = players.Where(x => !x.IsHost).ToArray();
                for (int i = 0; i < PlayerFields.Length; i++) {
                    if (i < nonHosts.Length) {
                        PlayerFields[i].text = nonHosts[i].Name;
                        PlayerReadyObjects[i].GetComponent<Image>().color = nonHosts[i].Ready
                            ? Color.green
                            : Color.red;
                    }
                    else {
                        PlayerFields[i].text = "";
                        PlayerReadyObjects[i].GetComponent<Image>().color = Color.clear;
                    }
                }
                LobbyButtonEvents events = ReadyStartButton.GetComponent<LobbyButtonEvents>();
                if (host.Id == session.OwnId) {
                    events.ButtonType = "Start";
                    if (players.Any(x => !x.Ready && !x.IsHost))
                        events.Enabled = false;
                    else
                        events.Enabled = true;
                }
                else {
                    events.ButtonType = "Ready";
                    events.Enabled = true;
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
