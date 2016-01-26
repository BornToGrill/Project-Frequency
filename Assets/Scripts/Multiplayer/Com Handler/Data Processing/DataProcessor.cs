using System;
using System.Linq;
using NetworkLibrary;

class DataProcessor {

    private readonly Invoke _invoke;

    public DataProcessor(IInvokable invoke) {
        _invoke = new Invoke(invoke);
    }

    internal void ProcessData(TcpClient client, string message) {
        string[] separated =
            message.Split(new[] { "]" }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.TrimStart('[')).ToArray();

        foreach (string command in separated) {
            SplitData data = command.GetFirst();

            switch (data.CommandType) {
                case "Invoke":
                    _invoke.HandleInvoke(client, data.Values);
                    break;
            }
        }

    }
}
