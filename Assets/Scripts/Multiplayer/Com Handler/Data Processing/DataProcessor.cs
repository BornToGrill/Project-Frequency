using System;
using System.Linq;
using NetworkLibrary;

class DataProcessor {

    private readonly Invokable _invokable;
    private readonly Notifiable _notifiable;

    public DataProcessor(IInvokable invoke, INotifiable notify) {
        _invokable = new Invokable(invoke);
        _notifiable = new Notifiable(notify);
    }

    internal void ProcessData(TcpClient client, string message) {
        string[] separated =
            message.Split(new[] { "]" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.TrimStart('[')).ToArray();

        foreach (string command in separated) {
            SplitData data = command.GetFirst();

            switch (data.CommandType) {
                case "Invoke":
                    _invokable.HandleInvoke(client, data.Values);
                    break;
                case "Notify":
                    _notifiable.HandleNotify(client, data.Values);
                    break;
                default:
                    UnityEngine.Debug.LogError("Invalid message send to DataProcessor\n" + message + "\n" +
                                               data.CommandType + "  with values : " + data.Values);
                    break;
            }
        }

    }
}
