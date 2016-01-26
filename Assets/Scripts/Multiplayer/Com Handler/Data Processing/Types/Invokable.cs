using System;
using System.Collections.Generic;
using System.Linq;
using NetworkLibrary;

class Invokable {

    private readonly IInvokable _invoke;
    private const char ValueDelimiter = '|';

    public Invokable(IInvokable invoke) {
        _invoke = invoke;
    }

    public void HandleInvoke(TcpClient client, string values) {
        SplitData data = values.GetFirst();

        switch (data.CommandType) {
            case "PlayerJoined":
                PlayerJoined(data.Values);
                break;
            case "SetPlayers":
                SetPlayers(data.Values);
                break;
            case "Authenticated":
                Authenticated(data.Values);
                break;
        }
    }

    private void PlayerJoined(string values) {
        string[] split = values.Split(ValueDelimiter);
        _invoke.PlayerConnected(Int32.Parse(split[0]), split[1]);
    }

    private void SetPlayers(string values) {
        string[] players =
            values.Split(new[] { ")" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.TrimStart('(')).ToArray();
        List<string> names = new List<string>();
        List<int> ids = new List<int>();
        foreach (string player in players) {
            string[] split = player.Split(ValueDelimiter);
            names.Add(split[0]);
            ids.Add(Int32.Parse(split[1]));
        }
        _invoke.SetPlayers(names.ToArray(), ids.ToArray());
    }

    private void Authenticated(string values) {
        string[] data = values.Split(ValueDelimiter);
        _invoke.Authenticated(data[0], Int32.Parse(data[1]));
    }

}