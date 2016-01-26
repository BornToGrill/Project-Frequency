using System;
using NetworkLibrary;

class Notifiable {

    private readonly INotifiable _notify;

    private const char ValueDelimiter = '|';

    public Notifiable(INotifiable notify) {
        _notify = notify;
    }

    public void HandleNotify(TcpClient client, string values) {
        SplitData data = values.GetFirst();

        switch (data.CommandType) {
            case "TurnEnd":
                EndTurn(data.Values);
                break;
            case "PlayerJoined":
                PlayerJoined(data.Values);
                break;
            default:
                UnityEngine.Debug.LogError("Invalid message send to Notify\n" + values);
                break;
        }
    }


    private void EndTurn(string values) {
        string[] data = values.Split(ValueDelimiter);
        _notify.EndTurn(data[1], Int32.Parse(data[0]));
    }
    private void PlayerJoined(string values) {
        string[] split = values.Split(ValueDelimiter);
        _notify.PlayerJoined(Int32.Parse(split[0]), split[1]);
    }
}
