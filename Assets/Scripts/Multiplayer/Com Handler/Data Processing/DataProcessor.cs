using System;
using System.Linq;
using NetworkLibrary;

class DataProcessor {

    private readonly Invokable _invokable;

    public DataProcessor(IInvokable invoke) {
        _invokable = new Invokable(invoke);
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
            }
        }

    }
}
