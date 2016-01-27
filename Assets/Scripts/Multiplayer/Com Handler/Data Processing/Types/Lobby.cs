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
            default:
                Debug.LogError("Invalid message send to Lobby\n" + values);
                break;
        }
    }

    private void PlayerJoined(string values) {
        string[] data = values.Split(ValueDelimiter);
        _lobby.PlayerJoined(Int32.Parse(data[0]), data[1]);
    }
}

