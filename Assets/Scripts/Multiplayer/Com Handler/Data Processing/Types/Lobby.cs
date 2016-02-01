using System;
using System.Collections.Generic;
using System.Linq;
using NetworkLibrary;
using UnityEngine;

class Lobby {

    private readonly ILobby _lobby;
    private const char ValueDelimiter = '|';

    public Lobby(ILobby lobby) {
        _lobby = lobby;
    }


    public void HandleLobby(TcpClient client, string values) {

        SplitData data = values.GetFirst();

        switch (data.CommandType) {
            case "StartGame":
                StartGame(data.Values);
                break;
            case "SetPlayers":
                SetPlayers(data.Values);
                break;
            default:
                Debug.LogError("Invalid message send to Lobby\n" + values);
                break;
        }
    }

    private void StartGame(string values) {
        string[] data = values.Split(new [] {")"}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim('(')).ToArray();
        TempPlayer[] players = new TempPlayer[data.Length];

        for (int i = 0; i < data.Length; i++) {
            string[] playerData = data[i].Split(ValueDelimiter);
            players[i] = new TempPlayer() {
                Id = Int32.Parse(playerData[1]),
                Name = playerData[0]
            };
        }
        _lobby.GameStart(players);
    }

    private void SetPlayers(string values) {
        string[] data = values.Split(ValueDelimiter);
        TempPlayer[] players = new TempPlayer[data.Length];
        for (int i = 0; i < players.Length; i++) {
            string info = data[i].Trim('(', ')');
            string[] splt = info.Split(':');
            players[i] = new TempPlayer() {
                Name = splt[0],
                Id = int.Parse(splt[1]),
                Ready = bool.Parse(splt[2]),
                IsHost = bool.Parse(splt[3])
            };
        }
        _lobby.SetPlayers(players);
    }
}

public class TempPlayer {

    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsHost { get; set; }
    public bool Ready { get; set; }
}

