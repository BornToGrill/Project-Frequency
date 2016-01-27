using System;
using System.Linq;
using NetworkLibrary;

class DataProcessor {

    private readonly Invokable _invokable;
    private readonly Notifiable _notifiable;
    private readonly Lobby _lobby;

    public DataProcessor(IInvokable invoke, INotifiable notify, ILobby lobby) {
        if(invoke != null)
            _invokable = new Invokable(invoke);
        if(notify != null)
            _notifiable = new Notifiable(notify);
        if (lobby != null)
            _lobby = new Lobby(lobby);
    }

    internal void ProcessData(TcpClient client, string message) {
        string[] separated =
            message.Split(new[] { "]" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.TrimStart('[')).ToArray();

        foreach (string command in separated) {
            SplitData data = command.GetFirst();

            switch (data.CommandType) {
                case "Invoke":
                    if (_invokable != null)
                        _invokable.HandleInvoke(client, data.Values);
                    break;
                case "Notify":
                    if (_notifiable != null)
                        _notifiable.HandleNotify(client, data.Values);
                    break;
                case "Lobby":
                    if (_lobby != null)
                        _lobby.HandleLobby(client, data.Values);
                    break;
                default:
                    UnityEngine.Debug.LogError("Invalid message send to DataProcessor\n" + message + "\n" +
                                               data.CommandType + "  with values : " + data.Values);
                    break;
            }
        }

    }
}
