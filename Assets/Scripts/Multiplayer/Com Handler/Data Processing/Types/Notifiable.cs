using System;
using System.Linq;
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
            case "GameWon":
                GameWon(data.Values);
                break;
            case "GameLoaded":
                _notify.GameLoaded();
                break;
            case "PlayerLeft":
                PlayerLeft(data.Values);
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

    private void GameWon(string values) {
        int[] data = values.Split(new [] {ValueDelimiter.ToString()}, StringSplitOptions.RemoveEmptyEntries).Select(x => Int32.Parse(x)).ToArray();
        if (data.Length > 1)
            _notify.GameWon(data[0], data.Skip(1).ToArray());
        else
            _notify.GameWon(data[0], null);
    }

    private void PlayerLeft(string values) {
        string[] data = values.Split(ValueDelimiter);
        _notify.PlayerLeft(Int32.Parse(data[0]), data[1]);
    }
}
