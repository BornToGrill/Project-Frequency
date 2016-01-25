using NetworkLibrary;

class DataProcessor {

    private readonly Invoke _invoke;

    public DataProcessor(IInvokable invoke) {
        _invoke = new Invoke(invoke);
    }

    internal void ProcessData(TcpClient client, string message) {
        SplitData data = message.GetFirst();

        switch (data.CommandType) {
            case "Invoke":
                _invoke.HandleInvoke(client, data.Values);
                break;
        }

    }
}
