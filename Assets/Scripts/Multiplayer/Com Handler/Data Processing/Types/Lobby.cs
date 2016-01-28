using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            case "PlayerJoined":
                PlayerJoined(data.Values);
                break;
            case "PlayerLeft":
                PlayerLeft(data.Values);
                break;
            case "GameStart":
                GameStart(data.Values);
                break;
            case "Authenticated":
                Authenticate(data.Values);
                break;
            default:
                Debug.LogError("Invalid message send to Lobby\n" + values);
                break;
        }
    }

    private void PlayerJoined(string values) {
        string[] data = values.Split(ValueDelimiter);
        _lobby.PlayerJoined(Int32.Parse(data[0]), data[1]);
    }

    private void PlayerLeft(string values) {
        string[] data = values.Split(ValueDelimiter);
        _lobby.PlayerLeft(Int32.Parse(data[0]), data[1]);
    }

    private void GameStart(string values) {
        string[] data = values.Split(ValueDelimiter);
        TempPlayer[] players = new TempPlayer[data.Length];

        for (int i = 0; i < data.Length; i++) {
            string[] playerData = data[i].Split(':');
            players[i] = new TempPlayer() {
                Id = Int32.Parse(playerData[0]),
                Name = playerData[1]
            };
        }
        _lobby.GameStart(players);
    }

    private void Authenticate(string values) {
        string[] data = values.Split(ValueDelimiter);
        _lobby.Authenticated(data[0], Int32.Parse(data[1]));
    }
}

public class TempPlayer {

    public int Id { get; set; }
    public string Name { get; set; }
}

