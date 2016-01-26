using System;
using NetworkLibrary;

class Notify {

    private INotifiable _notify;

    private const char ValueDelimiter = '|';

    public Notify(INotifiable notify) {
        _notify = notify;
    }

    public void HandleInvoke(TcpClient client, string values) {
        SplitData data = values.GetFirst();

        switch (data.CommandType) {
            case "TurnEnd":
                EndTurn(data.Values);
                break;
        }
    }


    private void EndTurn(string values) {
        string[] data = values.Split(ValueDelimiter);
        _notify.EndTurn(data[1], Int32.Parse(data[0]));

    }
}
